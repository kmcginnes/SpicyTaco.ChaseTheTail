using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Platform.VirtualFileSystem;

namespace ChaseTheTail
{
    public class TailService
    {
        public IObservable<string> Tail(IFile file)
        {
            return Observable.Create<string>(async subj =>
            {
                var disposable = new CompositeDisposable();

                // read all text from file
                string content;
                long lastMaxOffset = 0;
                using (var reader = new StreamReader(file.GetContent().GetInputStream(FileMode.Open, FileShare.ReadWrite)))
                {
                    lastMaxOffset = reader.BaseStream.Length;
                    content = await reader.ReadToEndAsync();
                }
                subj.OnNext(content);

                NodeActivityEventHandler onActivity = (sender, args) =>
                {
                    using (var reader = new StreamReader(file.GetContent().GetInputStream(FileMode.Open, FileShare.ReadWrite)))
                    {
                        //seek to the last max offset
                        reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);

                        //read out of the file until the EOF
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                            subj.OnNext(line);

                        //update the last max offset
                        lastMaxOffset = reader.BaseStream.Position;
                    }
                };
                file.Activity += onActivity;

                disposable.Add(Disposable.Create(() => file.Activity -= onActivity));
                
                return disposable;
            });
        }
    }
}