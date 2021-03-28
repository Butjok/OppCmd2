using System;
using Bulka.CommandLine.Utilities;
using UnityEngine;

namespace Bulka.CommandLine.Gui
{
    // https://draculatheme.com/contribute
    
    [CLSCompliant(false)]
    public static class Dracula
    {
        public static readonly Color background = "#282a36".ParseHexColor();
        public static readonly Color currentLine = "#44475a".ParseHexColor();
        public static readonly Color foreground = "#f8f8f2".ParseHexColor();
        public static readonly Color comment = "#6272a4".ParseHexColor();
        public static readonly Color cyan = "#8be9fd".ParseHexColor();
        public static readonly Color green = "#50fa7b".ParseHexColor();
        public static readonly Color orange = "#ffb86c".ParseHexColor();
        public static readonly Color pink = "#ff79c6".ParseHexColor();
        public static readonly Color purple = "#bd93f9".ParseHexColor();
        public static readonly Color red = "#ff5555".ParseHexColor();
        public static readonly Color yellow = "#f1fa8c".ParseHexColor();
    }
}