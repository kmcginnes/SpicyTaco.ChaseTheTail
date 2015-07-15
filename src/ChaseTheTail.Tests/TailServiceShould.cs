using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Platform;
using Platform.VirtualFileSystem;
using Platform.VirtualFileSystem.Providers;
using Platform.VirtualFileSystem.Providers.Imaginary;
using Xunit;

namespace ChaseTheTail.Tests
{
    public class TailShould
    {
        [Fact]
        public void StartWithEntireFileContentsWhenCalled()
        {
            var fs = new ImaginaryFileSystem("imaginary");

            var file = fs.ResolveFile("/test.log");
            file.Create();
            using (var writer = file.GetContent().GetWriter())
            {
                writer.Write("Some fake text");
            }

            var sut = new TailService();

            sut.Tail(file)
                .Take(1)
                .Subscribe(x => x.Should().Be("Some fake text"));
        }
    }
}
