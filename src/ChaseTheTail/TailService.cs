using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Platform.VirtualFileSystem;
using ReactiveUI;

namespace ChaseTheTail
{
    public class TailService
    {
        public IObservable<string> Tail(IFile file, IScheduler scheduler = null)
        {
            return Observable.Create<string>(subj =>
            {
                var disposable = new CompositeDisposable();
                scheduler = scheduler ?? RxApp.TaskpoolScheduler;
                var abortSignal = new ManualResetEvent(false);

                disposable.Add(Disposable.Create(() => abortSignal.Set()));

                disposable.Add(scheduler.Schedule(abortSignal, (sched, state) =>
                {
                    using (var reader = new StreamReader(
                        file.GetContent().OpenStream(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    {
                        long lastOffset = reader.BaseStream.Length;
                        if (reader.BaseStream.Length > 0)
                        {
                            // Send the last 10 kb of text to the reader.
                            lastOffset = Math.Max(0, reader.BaseStream.Length - (1024 * 10));
                        }

                        while (!state.WaitOne(100))
                        {
                            // Idle if file hasn't changed.
                            if (reader.BaseStream.Length <= lastOffset)
                            {
                                if (reader.BaseStream.Length < lastOffset)
                                {
                                    lastOffset = reader.BaseStream.Length;
                                }
                                continue;
                            }

                            // Read the data.
                            reader.BaseStream.Seek(lastOffset, SeekOrigin.Begin);
                            var delta = reader.BaseStream.Length - lastOffset;
                            var buffer = new char[delta];
                            reader.ReadBlock(buffer, 0, buffer.Length);

                            // Publish the data.
                            subj.OnNext(new string(buffer));

                            // Update the offset.
                            lastOffset = reader.BaseStream.Position;
                        }
                    }
                    return Disposable.Empty;
                }));

                return disposable;
            });
        }
    }
}