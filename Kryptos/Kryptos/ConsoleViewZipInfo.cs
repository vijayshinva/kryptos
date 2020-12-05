using System;
using System.Collections.Generic;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Kryptos
{
    class ConsoleViewZipInfo : StackLayoutView
    {
        protected TextSpanFormatter Formatter { get; } = new TextSpanFormatter();
        
        public ConsoleViewZipInfo(ZipArchive zipArchive)
        {
            var tableView = new TableView<ZipArchiveEntry>
            {
                Items = zipArchive.Entries
            };
            tableView.AddColumn(cellValue: e => e.Name, header: new ContentView("Name".Underline()));
            tableView.AddColumn(cellValue: e => e.Length, header: new ContentView("Length".Underline()));
            tableView.AddColumn(cellValue: e => e.CompressedLength, header: new ContentView("CompressedLength".Underline()));
            tableView.AddColumn(cellValue: e => e.Crc32, header: new ContentView("Crc32".Underline()));
            Add(tableView);
        }
    }
}
