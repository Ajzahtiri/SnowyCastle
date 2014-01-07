using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Storage;

namespace Snowy_Castle
{
    [Serializable]
    public struct HighScoreData
    {
        public string[] name;
        public int[] score;
        public int count;

        public HighScoreData(int c)
        {
            name = new String[c];
            score = new int[c];
            this.count = c;
        }
    }
}
