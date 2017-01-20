using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BicycleCalculatorWPF
{
    public class CData
    {
        public CGearLists frlists = new CGearLists(Properties.Resources.StringCranksets, GearType.Front);
        public CGearLists bklists = new CGearLists(Properties.Resources.StringCassette, GearType.Back);
        public CGearLists inlists = new CGearLists(Properties.Resources.StringInternalHub, GearType.Inner);
        
        public List<CWheel> wheelList = new List<CWheel>();


        public List<CRim> rimlist = new List<CRim>();
        public List<CHub> hublist = new List<CHub>();

        public List<CRim> rimlist_filtered = new List<CRim>();
        public List<CHub> hublist_filtered = new List<CHub>();

        public CData()
        {
            frlists.Lists.Add(new CGearList("1 Single", 1));
            frlists.Lists.Add(new CGearList("2 Double", 2));
            frlists.Lists.Add(new CGearList("3 Triple", 3));

            bklists.Lists.Add(new CGearList("1s/" + Properties.Resources.StringInternalHub, 1));
            bklists.Lists.Add(new CGearList("2s/" + Properties.Resources.StringInternalHub, 2));
            bklists.Lists.Add(new CGearList("3s", 3));
            bklists.Lists.Add(new CGearList("6s", 6));
            bklists.Lists.Add(new CGearList("7s", 7));
            bklists.Lists.Add(new CGearList("8s", 8));
            bklists.Lists.Add(new CGearList("9s", 9));
            bklists.Lists.Add(new CGearList("10s", 10));
            bklists.Lists.Add(new CGearList("11s", 11));
            bklists.Lists.Add(new CGearList("12s", 12));

            inlists.Lists.Add(new CGearList(Properties.Resources.StringNone, 1));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "2", 2));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "3", 3));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "4", 4));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "5", 5));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "6", 6));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "7", 7));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "8", 8));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "9", 9));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "10", 10));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "11", 11));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "12", 12));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "13", 13));
            inlists.Lists.Add(new CGearList(Properties.Resources.StringInner + "14", 14));

        }

        string rim_last_key = "#####";
        public bool FilterRimData(string KeyWord)
        {
            if (rim_last_key == KeyWord) return false;
            rim_last_key = KeyWord;
            rimlist_filtered.Clear();
            if (KeyWord == "")
            {
                foreach (CRim item in rimlist)
                    rimlist_filtered.Add(item);
                return true;
            }
            foreach (CRim item in rimlist)
            {
                string[] KeyWords = KeyWord.Split(' ');
                bool Add = true;
                foreach (string k in KeyWords)
                {
                    if (!item.Name.ToLower().Contains(k.ToLower()))
                    {
                        Add = false;
                        break;
                    }
                }
                if (Add) rimlist_filtered.Add(item);
            }
            return true;
        }
        string hub_last_key = "#####";
        public bool FilterHubData(string KeyWord)
        {
            if (hub_last_key == KeyWord) return false;
            hub_last_key = KeyWord;
            hublist_filtered.Clear();
            if (KeyWord == "")
            {
                foreach (CHub item in hublist)
                    hublist_filtered.Add(item);
                return true;
            }
            foreach (CHub item in hublist)
            {
                string[] KeyWords = KeyWord.Split(' ');
                bool Add = true;
                foreach (string k in KeyWords)
                {
                    if (!item.Name.ToLower().Contains(k.ToLower()))
                    {
                        Add = false;
                        break;
                    }
                }
                if (Add) hublist_filtered.Add(item);
            }
            return true;
        }

        public void SaveDate()
        {
            System.IO.FileStream fs = new System.IO.FileStream("Data.csv", System.IO.FileMode.Create);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, Encoding.UTF8);
            //开始写入
            foreach (CGearList list in frlists.Lists)
            {
                foreach (CGear gear in list.Gears)
                {
                    sw.WriteLine(gear.Encode());
                }
            }
            foreach (CGearList list in bklists.Lists)
            {
                foreach (CGear gear in list.Gears)
                {
                    sw.WriteLine(gear.Encode());
                }
            }
            foreach (CGearList list in inlists.Lists)
            {
                foreach (CGear gear in list.Gears)
                {
                    sw.WriteLine(gear.Encode());
                }
            }
            foreach (CWheel wheel in wheelList)
            {
                sw.WriteLine(wheel.Encode());
            }
            foreach (CHub hub in hublist)
            {
                sw.WriteLine(hub.Encode());
            }
            foreach (CRim rim in rimlist)
            {
                sw.WriteLine(rim.Encode());
            }
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public void AddGear(CGear gear)
        {
            switch (gear.Type)
            {
                case GearType.Front:
                    foreach (CGearList list in frlists.Lists)
                    {
                        if (gear.Speeds == list.speeds)
                        {
                            gear.Parent = list;
                            list.Gears.Add(gear);
                        }
                    }
                    break;
                case GearType.Back:
                    foreach (CGearList list in bklists.Lists)
                    {
                        if (gear.Speeds == list.speeds)
                        {
                            gear.Parent = list;
                            list.Gears.Add(gear);
                        }
                    }
                    break;
                case GearType.Inner:
                    foreach (CGearList list in inlists.Lists)
                    {
                        if (gear.Speeds == list.speeds)
                        {
                            gear.Parent = list;
                            list.Gears.Add(gear);
                        }
                    }
                    break;
            }
        }

        public void AddGear(List<CGear> gears)
        {
            foreach (CGear gear in gears)
            {
                switch (gear.Type)
                {
                    case GearType.Front:
                        foreach (CGearList list in frlists.Lists)
                        {
                            if (gear.Speeds == list.speeds)
                            {
                                gear.Parent = list;
                                list.Gears.Add(gear);
                            }
                        }
                        break;
                    case GearType.Back:
                        foreach (CGearList list in bklists.Lists)
                        {
                            if (gear.Speeds == list.speeds)
                            {
                                gear.Parent = list;
                                list.Gears.Add(gear);
                            }
                        }
                        break;
                    case GearType.Inner:
                        foreach (CGearList list in inlists.Lists)
                        {
                            if (gear.Speeds == list.speeds)
                            {
                                gear.Parent = list;
                                list.Gears.Add(gear);
                            }
                        }
                        break;
                }
            }
        }

        public void ClearAll()
        {
            foreach (CGearList list in frlists.Lists)
            {
                list.Gears.Clear();
            }
            foreach (CGearList list in bklists.Lists)
            {
                list.Gears.Clear();
            }
            foreach (CGearList list in inlists.Lists)
            {
                list.Gears.Clear();
            }
            wheelList.Clear();
            hublist.Clear();
            rimlist.Clear();
        }

        public void LoadData()
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream("Data.csv", System.IO.FileMode.Open);
                System.IO.StreamReader sr = new System.IO.StreamReader(fs, Encoding.UTF8);
                String line;
                List<CGear> gearlisttemp = new List<CGear>();
                List<CWheel> wheellisttemp = new List<CWheel>();
                List<CHub> hublisttemp = new List<CHub>();
                List<CRim> rimlisttemp = new List<CRim>();

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("Gear"))
                    {
                        gearlisttemp.Add(new CGear(line));
                    }
                    if (line.StartsWith("Wheel"))
                    {
                        wheellisttemp.Add(new CWheel(line));
                    }
                    if (line.StartsWith("Hub"))
                    {
                        hublisttemp.Add(new CHub(line));
                    }
                    if (line.StartsWith("Rim"))
                    {
                        rimlisttemp.Add(new CRim(line));
                    }
                }

                ClearAll();

                AddGear(gearlisttemp);
                
                foreach (CWheel wheel in wheellisttemp)
                {
                    wheelList.Add(wheel);
                }

                foreach (CHub hub in hublisttemp)
                {
                    hublist.Add(hub);
                }
                foreach (CRim rim in rimlisttemp)
                {
                    rimlist.Add(rim);
                }

                FilterRimData("");
                FilterHubData("");
                //System.Windows.MessageBox.Show("读取数据完成。");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(Properties.Resources.StringFailReadFile + "\r\n" + ex.Message);
            }

        }

        public void DataInit()
        {
            LoadData();

            //LoadRawData();

            //SaveDate();
        }

        private void LoadRawData()
        {

            frlists.Lists[0].Gears.Add(new CGear(GearType.Front, "42T", 1, 42, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[0].Gears.Add(new CGear(GearType.Front, "44T", 1, 44, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[0].Gears.Add(new CGear(GearType.Front, "46T", 1, 46, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[0].Gears.Add(new CGear(GearType.Front, "48T", 1, 48, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[0].Gears.Add(new CGear(GearType.Front, "50T", 1, 50, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[0].Gears.Add(new CGear(GearType.Front, "52T", 1, 52, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[0].Gears.Add(new CGear(GearType.Front, "53T", 1, 53, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[0].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringOther, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            frlists.Lists[1].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringMTB + "39/27T", 2, 27, 39, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[1].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "50/34T", 2, 34, 50, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[1].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "50/36T", 2, 36, 50, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[1].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "52/36T", 2, 36, 52, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[1].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "53/39T", 2, 39, 53, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[1].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "52/39T", 2, 39, 52, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[1].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "56/46T", 2, 46, 56, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[1].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "56/42T", 2, 42, 56, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[1].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringOther, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            frlists.Lists[2].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringMTB + "42/32/24T", 3, 24, 32, 42, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[2].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringMTB + "42/34/24T", 3, 24, 34, 42, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[2].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringMTB + "48/38/28T", 3, 28, 38, 48, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[2].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "52/39/30T", 3, 30, 39, 52, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[2].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "50/39/30T", 3, 30, 39, 50, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[2].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringRacing + "52/42/30T", 3, 30, 42, 52, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            frlists.Lists[2].Gears.Add(new CGear(GearType.Front, Properties.Resources.StringOther, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[0].Gears.Add(new CGear(GearType.Back, "18T", 1, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            bklists.Lists[0].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[1].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[2].Gears.Add(new CGear(GearType.Back, "Brompton", 3, 17, 15, 13, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            bklists.Lists[2].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[3].Gears.Add(new CGear(GearType.Back, "14-28T", 6, 28, 24, 21, 18, 16, 14, 1, 1, 1, 1, 1, 1));
            bklists.Lists[3].Gears.Add(new CGear(GearType.Back, "14-34T", 6, 34, 24, 21, 18, 16, 14, 1, 1, 1, 1, 1, 1));
            bklists.Lists[3].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[4].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "14-34T", 7, 34, 24, 22, 20, 18, 16, 14, 1, 1, 1, 1, 1));
            bklists.Lists[4].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "12-32T", 7, 32, 26, 21, 18, 16, 14, 12, 1, 1, 1, 1, 1));
            bklists.Lists[4].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-28T", 7, 28, 24, 21, 18, 15, 13, 11, 1, 1, 1, 1, 1));
            bklists.Lists[4].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "14-28T", 7, 28, 24, 22, 20, 18, 16, 14, 1, 1, 1, 1, 1));
            bklists.Lists[4].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[5].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "11-32T", 8, 32, 28, 24, 21, 18, 15, 13, 11, 1, 1, 1, 1));
            bklists.Lists[5].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "13-34T", 8, 34, 28, 24, 21, 19, 17, 15, 13, 1, 1, 1, 1));
            bklists.Lists[5].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "12-23T", 8, 23, 21, 19, 17, 15, 14, 13, 12, 1, 1, 1, 1));
            bklists.Lists[5].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-23T", 8, 23, 21, 19, 17, 15, 13, 12, 11, 1, 1, 1, 1));
            bklists.Lists[5].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "12-25T", 8, 25, 23, 21, 19, 17, 15, 13, 12, 1, 1, 1, 1));
            bklists.Lists[5].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-28T", 8, 28, 24, 21, 19, 17, 15, 13, 11, 1, 1, 1, 1));
            bklists.Lists[5].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-30T", 8, 30, 26, 23, 20, 17, 15, 13, 11, 1, 1, 1, 1));
            bklists.Lists[5].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[6].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "11-32T", 9, 32, 28, 24, 21, 18, 16, 14, 12, 11, 1, 1, 1));
            bklists.Lists[6].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "13-32T", 9, 32, 28, 24, 21, 18, 16, 15, 14, 13, 1, 1, 1));
            bklists.Lists[6].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "11-34T", 9, 34, 28, 24, 21, 18, 16, 14, 12, 11, 1, 1, 1));
            bklists.Lists[6].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "11-36T", 9, 36, 32, 28, 24, 21, 18, 15, 13, 11, 1, 1, 1));
            bklists.Lists[6].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-25T", 9, 25, 23, 21, 19, 17, 15, 13, 12, 11, 1, 1, 1));
            bklists.Lists[6].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-28T", 9, 28, 24, 21, 18, 16, 14, 13, 12, 11, 1, 1, 1));
            bklists.Lists[6].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "12-27T", 9, 27, 24, 21, 19, 17, 15, 14, 13, 12, 1, 1, 1));
            bklists.Lists[6].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[7].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "11-34T", 10, 34, 30, 26, 23, 21, 19, 17, 15, 13, 11, 1, 1));
            bklists.Lists[7].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "11-36T", 10, 36, 32, 28, 24, 21, 19, 17, 15, 13, 11, 1, 1));
            bklists.Lists[7].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "11-36T PG-1070", 10, 36, 32, 28, 25, 22, 19, 17, 15, 13, 11, 1, 1));
            bklists.Lists[7].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-25T", 10, 25, 23, 21, 19, 17, 15, 14, 13, 12, 11, 1, 1));
            bklists.Lists[7].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-28T", 10, 28, 25, 22, 19, 17, 15, 14, 13, 12, 11, 1, 1));
            bklists.Lists[7].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-28T ultegra", 10, 28, 24, 21, 19, 17, 15, 14, 13, 12, 11, 1, 1));
            bklists.Lists[7].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "12-27T", 10, 27, 24, 21, 19, 17, 16, 15, 14, 13, 12, 1, 1));
            bklists.Lists[7].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 10, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[8].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringMTB + "10-42T", 11, 42, 36, 32, 28, 24, 21, 18, 16, 14, 12, 10, 1));
            bklists.Lists[8].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-23T", 11, 23, 21, 19, 18, 17, 16, 15, 14, 13, 12, 11, 1));
            bklists.Lists[8].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-25T", 11, 25, 23, 21, 19, 17, 16, 15, 14, 13, 12, 11, 1));
            bklists.Lists[8].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-26T", 11, 26, 23, 21, 19, 17, 16, 15, 14, 13, 12, 11, 1));
            bklists.Lists[8].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-28T", 11, 28, 25, 23, 21, 19, 17, 15, 14, 13, 12, 11, 1));
            bklists.Lists[8].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "11-32T", 11, 32, 28, 25, 22, 20, 18, 16, 14, 13, 12, 11, 1));
            bklists.Lists[8].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringRacing + "12-25T", 11, 25, 23, 21, 19, 18, 17, 16, 15, 14, 13, 12, 1));
            bklists.Lists[8].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            bklists.Lists[9].Gears.Add(new CGear(GearType.Back, Properties.Resources.StringOther, 12, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));


            inlists.Lists[0].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringNone, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[1].Gears.Add(new CGear(GearType.Inner, "SRAM AUTOMATIX", 2, 1, 1.37, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[1].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[2].Gears.Add(new CGear(GearType.Inner, "Nexus SG-3S40", 3, 0.75, 1, 1.37, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[2].Gears.Add(new CGear(GearType.Inner, "SRAM SPECTRO T3", 3, 0.73, 1, 1.36, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[2].Gears.Add(new CGear(GearType.Inner, "SRAM DualDrive3", 3, 0.73, 1, 1.36, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[2].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[3].Gears.Add(new CGear(GearType.Inner, "SRAM SPARC", 4, 0.630, 0.780, 1.000, 1.280, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[3].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[4].Gears.Add(new CGear(GearType.Inner, "SRAM SPECTRO P5", 5, 0.630, 0.780, 1.000, 1.280, 1.580, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[4].Gears.Add(new CGear(GearType.Inner, "SRAM SPECTRO P5 CARGO", 5, 0.670, 0.780, 1.000, 1.280, 1.500, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[4].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[5].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[6].Gears.Add(new CGear(GearType.Inner, "Nexus SG-7R46", 7, 0.632, 0.741, 0.843, 0.989, 1.145, 1.335, 1.545, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[6].Gears.Add(new CGear(GearType.Inner, "SRAM Spectro S7", 7, 0.570, 0.680, 0.810, 1.000, 1.240, 1.480, 1.740, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[6].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[7].Gears.Add(new CGear(GearType.Inner, "Nexus SG-8R(C)31", 8, 0.527, 0.644, 0.748, 0.851, 1.00, 1.223, 1.419, 1.615, 1, 1, 1, 1, 1, 1));
            inlists.Lists[7].Gears.Add(new CGear(GearType.Inner, "ALFINE SG-S501", 8, 0.527, 0.644, 0.748, 0.851, 1.00, 1.223, 1.419, 1.615, 1, 1, 1, 1, 1, 1));
            inlists.Lists[7].Gears.Add(new CGear(GearType.Inner, "SRAM G8", 8, 0.609, 0.710, 0.803, 0.903, 1.054, 1.204, 1.355, 1.581, 1, 1, 1, 1, 1, 1));
            inlists.Lists[7].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[8].Gears.Add(new CGear(GearType.Inner, "SRAM G9", 9, 0.541, 0.609, 0.710, 0.803, 0.903, 1.054, 1.204, 1.355, 1.581, 1, 1, 1, 1, 1));
            inlists.Lists[8].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[9].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 10, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[10].Gears.Add(new CGear(GearType.Inner, "ALFINE SG-S700", 11, 0.527, 0.681, 0.770, 0.878, 0.995, 1.134, 1.292, 1.462, 1.667, 1.888, 2.153, 1, 1, 1));
            inlists.Lists[10].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[11].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 12, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
            inlists.Lists[12].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 13, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

            inlists.Lists[13].Gears.Add(new CGear(GearType.Inner, "Rohloff Speedhub", 14, 0.279, 0.316, 0.360, 0.409, 0.464, 0.528, 0.600, 0.682, 0.774, 0.881, 1.000, 1.135, 1.292, 1.467));
            inlists.Lists[13].Gears.Add(new CGear(GearType.Inner, Properties.Resources.StringOther, 14, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));


            wheelList.Add(new CWheel("37-298 14x1 1/4", 1169));
            wheelList.Add(new CWheel("37-298 14x1 3/8", 1169));
            wheelList.Add(new CWheel("37-254 14x1.35", 1030));
            wheelList.Add(new CWheel("54-254 14x2.1", 1137));

            wheelList.Add(new CWheel("32-349 16x1 1/4 KOJAK", 1297));
            wheelList.Add(new CWheel("35-349 16x1.35 MARATHON", 1316));
            wheelList.Add(new CWheel("37-340 16x1 1/2", 1301));
            wheelList.Add(new CWheel("44-317 16x1 3/4", 1272));
            wheelList.Add(new CWheel("37-335 16x1 3/8", 1285));
            wheelList.Add(new CWheel("44-305 16x1.75", 1235));
            wheelList.Add(new CWheel("44-305 16x1.75", 1235));
            wheelList.Add(new CWheel("47-305 16x1.75", 1272));

            wheelList.Add(new CWheel("28-355 18x1.125", 1291));
            wheelList.Add(new CWheel("32-355 18x1.25 KOJAK", 1316));
            wheelList.Add(new CWheel("35-355 18x1.35 MARATHON", 1335));
            wheelList.Add(new CWheel("44-355 18x1.5", 1392));
            wheelList.Add(new CWheel("47-355 18x1.75", 1411));

            wheelList.Add(new CWheel("19-451 20x1", 1536));
            wheelList.Add(new CWheel("37-438 20x1 1/2", 1608));
            wheelList.Add(new CWheel("40-432 20x1 1/2", 1608));
            wheelList.Add(new CWheel("32-445 20x1 1/4", 1599));
            wheelList.Add(new CWheel("32-451 20x1 1/4", 1618));
            wheelList.Add(new CWheel("33-451 20x1 1/4", 1624));
            wheelList.Add(new CWheel("28-451 20x1 1/8", 1593));
            wheelList.Add(new CWheel("28-451 20x1 1/8", 1593));

            wheelList.Add(new CWheel("28-451 20x1 1/8", 1593));

            wheelList.Add(new CWheel("28-406 20x1.1 DURANO", 1451));
            wheelList.Add(new CWheel("35-406 20x1.35 KOJAK", 1495));
            wheelList.Add(new CWheel("35-406 20x1.35 MARATHON", 1495));
            wheelList.Add(new CWheel("44-406 20x1.5", 1552));
            wheelList.Add(new CWheel("47-406 20x1.75 MARATHON", 1571));
            wheelList.Add(new CWheel("50-406 20x2.0 BIG APPLE", 1590));
            wheelList.Add(new CWheel("54-406 20x2.0", 1615));

            wheelList.Add(new CWheel("23-451 21x1.0", 1272));
            wheelList.Add(new CWheel("25-451 21x1.1", 1272));
            wheelList.Add(new CWheel("28-451 21x1.125", 1272));
            wheelList.Add(new CWheel("32-451 21x1.25", 1272));
            wheelList.Add(new CWheel("37-451 21x1.35", 1272));

            wheelList.Add(new CWheel("37-540 24x1 3/8", 1948));
            wheelList.Add(new CWheel("47-507 24x1.75", 1900));

            wheelList.Add(new CWheel("37-584 26x1 3/8 650STD", 2086));
            wheelList.Add(new CWheel("37-590 26x1 3/8 650x35A", 2100));
            wheelList.Add(new CWheel("28-559 26x1.10 DURANO", 1932));
            wheelList.Add(new CWheel("35-559 26x1.35 MARATHON", 1976));
            wheelList.Add(new CWheel("40-559 26x1.50 MARATHON", 2007));
            wheelList.Add(new CWheel("42-559 26x1.60", 2025));
            wheelList.Add(new CWheel("47-559 26x1.75 MARATHON", 2051));
            wheelList.Add(new CWheel("50-559 26x2.00", 2075));
            wheelList.Add(new CWheel("54-559 26x2.10", 2100));
            wheelList.Add(new CWheel("57-559 26x2.25", 2120));


            wheelList.Add(new CWheel("57-584 27 1/2x2.25", 2128));
            wheelList.Add(new CWheel("28-630 27x1 1/4", 2174));
            wheelList.Add(new CWheel("32-630 27x1 1/4", 2220));
            wheelList.Add(new CWheel("40-635 28x1 1/2", 2265));
            wheelList.Add(new CWheel("32-622 28x1.25 700x32C", 2170));
            wheelList.Add(new CWheel("35-622 28x1.35 700x35C", 2185));
            wheelList.Add(new CWheel("37-622 28x1.40 700x35C", 2200));
            wheelList.Add(new CWheel("37-622 28x1.40 700x37C", 2200));
            wheelList.Add(new CWheel("40-622 28x1.50 700x38C", 2220));
            wheelList.Add(new CWheel("42-622 28x1.60 700x40C", 2230));
            wheelList.Add(new CWheel("47-622 28x1.75 MARATHON", 2249));
            wheelList.Add(new CWheel("50-622 29x2.00", 2280));
            wheelList.Add(new CWheel("54-622 29x2.10", 2295));
            wheelList.Add(new CWheel("57-622 29x2.25", 2288));
            wheelList.Add(new CWheel("60-622 29x2.35", 2330));
            wheelList.Add(new CWheel("23-571 650x23C", 1973));
            wheelList.Add(new CWheel("18-622 700x18C", 2102));
            wheelList.Add(new CWheel("20-622 700x20C", 2100));
            wheelList.Add(new CWheel("23-622 700x23C", 2125));
            wheelList.Add(new CWheel("25-622 700x25C", 2135));
            wheelList.Add(new CWheel("28-622 700x28C", 2150));

            hublist.Add(new CHub(Properties.Resources.StringOther, 60, 60, 28, 28, 2.5));
            hublist.Add(new CHub("AC APX Front 100", 42.5, 42.5, 36.0, 36.0, 2.6));
            hublist.Add(new CHub("AC APX Rear 135", 48.0, 48.0, 35.0, 20.0, 2.6));
            hublist.Add(new CHub("ACCESS 4培林 Rear 130", 49.0, 49.0, 35.0, 19.0, 2.6));
            hublist.Add(new CHub("American Classic HD-140 disc Front 100", 56.5, 56.5, 23.0, 36.0, 2.6));
            hublist.Add(new CHub("American Classic HD-240 Ultra-Light 135mm Disk Rear 135", 66.2, 66.2, 29.0, 19.0, 2.6));
            hublist.Add(new CHub("American Classic HH-120 suspension Front 100", 47.0, 47.0, 36.0, 36.0, 2.6));
            hublist.Add(new CHub("American Classic Micro newer wider version Front 100", 30.0, 30.0, 33.5, 33.5, 2.6));
            hublist.Add(new CHub("American Classic Rear 126", 42.0, 42.0, 38.5, 18.5, 2.4));
            hublist.Add(new CHub("American Classic Rear 130", 42.0, 42.0, 36.5, 20.5, 2.4));
            hublist.Add(new CHub("American Classic Rear 130", 42.0, 42.0, 41.0, 16.0, 2.4));
            hublist.Add(new CHub("American Classic Rear 135", 42.0, 42.0, 34.0, 23.0, 2.4));
            hublist.Add(new CHub("American Classic Rear 135", 42.0, 42.0, 39.0, 18.0, 2.4));
            hublist.Add(new CHub("American Classic Rear 140", 42.0, 42.0, 36.5, 20.5, 2.4));
            hublist.Add(new CHub("American Classic Speedster older narrow version Front 100", 30.0, 30.0, 25.0, 25.0, 2.4));
            hublist.Add(new CHub("American Classic Suspension small flange Front 100", 47.0, 47.0, 36.0, 36.0, 2.4));
            hublist.Add(new CHub("American Classic track Front 100", 42.0, 42.0, 34.5, 34.5, 2.6));
            hublist.Add(new CHub("American Classic track Rear 121", 42.0, 42.0, 30.5, 30.5, 2.6));
            hublist.Add(new CHub("American Classic Ultra-Light 130mm Rear 130", 66.2, 66.2, 31.0, 17.0, 2.4));
            hublist.Add(new CHub("American Classic Ultra-Light 135mm Rear 135", 66.2, 66.2, 29.0, 19.0, 2.4));
            hublist.Add(new CHub("American Classic Ultra-Light CHECK DIAMETER! Rear 135", 42.0, 42.0, 29.0, 19.0, 2.4));
            hublist.Add(new CHub("Ariel Rear 135", 45.0, 45.0, 35.0, 20.0, 2.6));
            hublist.Add(new CHub("ASSESS(RIMNETE)4培林公路花鼓 Rear 130", 49.0, 49.0, 35.0, 19.0, 2.6));
            hublist.Add(new CHub("Bees Rear 135", 45.0, 45.0, 32.0, 19.0, 2.6));
            hublist.Add(new CHub("BikeE 16 hole disc hub Front 100", 62.2, 45.3, 20.2, 38.2, 2.4));
            hublist.Add(new CHub("Bontrager ATB PN 200128 Superstock Front 100", 38.0, 38.0, 34.1, 34.1, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 200129 Superstock Rear 135", 45.0, 45.0, 33.3, 20.3, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 200171 Race by Formula (semi-sealed bearings) Front 100", 38.0, 38.0, 35.0, 35.0, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 210240 Race Rear 135", 45.0, 45.0, 38.7, 20.3, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 210248 Race Mod Disc by DT Swiss (Onyx) Front 100", 58.0, 45.0, 23.3, 35.3, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 210249 Race Mod Disc by DT Swiss (Onyx) Rear 135", 58.0, 58.0, 33.8, 20.4, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220028 Select Disc Front 100", 58.0, 58.0, 22.4, 32.5, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220029 Select Disc Rear 135", 58.0, 58.0, 34.4, 20.4, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220196 Race by Formula (sealed bearings) Front 100", 42.0, 42.0, 32.8, 32.8, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220197 Race by Formula (sealed bearings) Rear 135", 45.0, 58.0, 38.5, 20.3, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220200 Race Disc by Formula Front 100", 58.0, 45.0, 16.7, 27.2, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220201 Race Disc by Formula Rear 135", 58.0, 58.0, 33.9, 20.3, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220628 Select Front 100", 38.0, 38.0, 34.1, 34.1, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220629 Select Rear 135", 45.0, 45.0, 33.3, 20.3, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220864 Race Lite by DT Rear 135", 44.0, 53.0, 39.0, 20.5, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220864 Race Lite by DT 130mm Rear 130", 44.0, 53.0, 36.5, 23.0, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220865 Race Modified by DT Swiss (Onyx) Rear 135", 45.0, 58.0, 38.5, 20.2, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220866 Race Lite by DT Front 100", 40.5, 40.5, 37.4, 37.4, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 220867 Race Modified by DT Swiss (Onyx)  Front 100", 38.0, 38.0, 35.0, 35.0, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 230044 sealed bearings Front 100", 38.0, 38.0, 35.0, 35.0, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 230515 sealed bearings disc Front 100", 45.0, 58.0, 36.3, 22.3, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 230516 sealed bearings disc Rear 135", 58.0, 58.0, 33.9, 20.3, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 240141 Disc Team Front 100", 58.0, 46.0, 22.4, 35.35, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 240175 Disc Team Rear 135", 58.0, 46.0, 34.0, 19.45, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 242146 Race X Lite centerlock Front 100", 47.0, 40.0, 22.8, 33.4, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 242147 Race X Lite centerlock Rear 135", 47.0, 47.0, 29.0, 24.7, 2.5));
            hublist.Add(new CHub("Bontrager ATB PN 990704 Yellow or red seals Rear 135", 45.0, 58.0, 38.6, 20.3, 2.6));
            hublist.Add(new CHub("Bontrager ATB PN Prototype LB/4 \"XT class\" Rear 135", 45.0, 45.0, 34.3, 19.8, 2.5));
            hublist.Add(new CHub("Bontrager ATB Race X-Lite by Chris King Front 100", 40.0, 40.0, 37.6, 37.6, 2.5));
            hublist.Add(new CHub("Bontrager ATB Race X-Lite by Chris King Rear 135", 44.0, 53.0, 36.0, 21.2, 2.5));
            hublist.Add(new CHub("Bontrager PN 242146 Race X Lite disc Front 100", 47.0, 40.0, 23.0, 34.2, 2.5));
            hublist.Add(new CHub("Bontrager PN 242146 Race X Lite disc Rear 135", 47.0, 47.0, 33.0, 21.3, 2.5));
            hublist.Add(new CHub("Bontrager Power-tap 16H hi/lo proto. Even spacing. Use 1x. Rear 130", 77.8, 63.0, 32.5, 17.0, 2.7));
            hublist.Add(new CHub("Bontrager Power-tap 16H hi/lo proto. Paired. Calc1.33x, build1x. Rear 130", 78.0, 66.0, 33.0, 20.0, 2.5));
            hublist.Add(new CHub("Bontrager Power-tap 24H hi/lo proto. Paired. Calc1.25x, build1x. Rear 130", 78.0, 66.0, 32.0, 17.5, 2.5));
            hublist.Add(new CHub("Bontrager Road 240097,8 proto Rear 130", 29.3, 47.0, 34.9, 17.3, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 220212 Race X-Lite. Rear 130", 47.0, 47.0, 29.6, 17.6, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 220213 Race Lite. Radial only. Front 100", 38.0, 38.0, 33.5, 33.5, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 220271 Race Lite. Rear 130", 47.0, 47.0, 30.0, 18.1, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 220407 Select Rear 130", 47.0, 47.0, 30.0, 17.0, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 220622 Select Front 100", 38.0, 38.0, 34.1, 34.1, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 220969 RXL Aero. Radial only. Front 100", 38.0, 38.0, 34.2, 34.2, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 220970 RXL Aero. Use 1.333 cross. Rear 130", 47.0, 47.0, 33.2, 19.3, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 221088 Race Lite Campy. Use  2.25 cross Rear 130", 47.0, 47.0, 30.8, 17.3, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 230036 Race Lite Tandem. Use 2.25 cross. Rear 145", 60.0, 60.0, 29.6, 24.6, 2.7));
            hublist.Add(new CHub("Bontrager Road PN 230037 Race Lite Tandem. Use 2.25 cross. Front 100", 60.0, 60.0, 35.5, 35.5, 2.7));
            hublist.Add(new CHub("Bontrager Road PN 230046 Race Lite. Radial only. Front 100", 38.0, 38.0, 32.3, 32.3, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 230291 Formula RXL 28H standard spacing Front 100", 38.0, 38.0, 34.1, 34.1, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 230416 Power Tap 16H. Use 1x. Rear 130", 77.8, 77.8, 30.0, 17.6, 2.7));
            hublist.Add(new CHub("Bontrager Road PN 230849, RXL road Rear 130", 47.0, 47.0, 30.1, 18.1, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 230850, RXL Aero Front 100", 38.0, 38.0, 34.9, 34.9, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 230851 RXL Aero. Use 1.333 cross. Rear 130", 47.0, 47.0, 33.7, 19.8, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 240001 prototype 122g Front 100", 33.3, 33.3, 43.1, 43.1, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 240036 Team climbing ft hub Front 100", 39.0, 39.0, 37.5, 37.5, 0.0));
            hublist.Add(new CHub("Bontrager Road PN 240097 RXXXL from DT Rear 130", 30.56, 47.0, 36.5, 18.0, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 240097,8 proto Rear 130", 29.3, 47.0, 34.9, 17.3, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 240134 Race. Converted to 145 Rear 145", 47.0, 47.0, 22.8, 25.3, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 240134 Race. Rear 130", 47.0, 47.0, 30.3, 17.8, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 240144 proto Front 100", 28.8, 28.8, 39.7, 39.7, 0.0));
            hublist.Add(new CHub("Bontrager Road PN 240144 RXXXL from DT Front 100", 29.1, 29.1, 38.07, 38.07, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 240441 RXL Aero. Use 1.333 cross. Rear 130", 47.0, 47.0, 33.7, 19.5, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 240603 Race Lite. Radial or cross ok. Front 100", 38.0, 38.0, 34.1, 34.1, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 242220 flangless RXXXL Front 100", 29.1, 29.1, 38.3, 38.3, 0.0));
            hublist.Add(new CHub("Bontrager Road PN 242221 flangless RXXXL - for DS calcs Rear 130", 30.6, 47.0, 37.0, 16.75, 2.5));
            hublist.Add(new CHub("Bontrager Road PN 242221 flangless RXXXL - for NDS calcs Front 100", 30.6, 47.0, 37.0, 16.75, 0.0));
            hublist.Add(new CHub("Bontrager Road PN 244144 tandem low flange by Formula Rear 145", 47.0, 47.0, 30.8, 26.80, 2.6));
            hublist.Add(new CHub("Bontrager Road PN xxxxxx prototype carbon shell Front 100", 28.2, 28.2, 38.1, 38.1, 0.0));
            hublist.Add(new CHub("Bontrager Road PN xxxxxx prototype carbon shell Rear 130", 30.9, 47.0, 33.8, 18.0, 2.5));
            hublist.Add(new CHub("Bontrager Track PN 240007 Rear 120", 47.0, 47.0, 33.3, 33.3, 2.5));
            hublist.Add(new CHub("Bontrager Track PN 240008 Front 100", 38.0, 38.0, 34.1, 34.1, 2.5));
            hublist.Add(new CHub("Brave Monster 48-hole BMX freehub 8/9 cogs shimano-compatible Rear 135", 57.5, 57.5, 25.0, 21.5, 2.4));
            hublist.Add(new CHub("Bullseye  Front 100", 42.5, 42.5, 36.5, 36.5, 2.4));
            hublist.Add(new CHub("Bullseye 126mm 6speed Rear 126", 42.5, 42.5, 38.0, 21.5, 2.4));
            hublist.Add(new CHub("Bullseye 130mm 7speed Rear 130", 42.5, 42.5, 40.0, 20.5, 2.4));
            hublist.Add(new CHub("Bullseye tandem  Front 100", 53.0, 53.0, 36.5, 36.5, 2.4));
            hublist.Add(new CHub("Bullseye tandem  Rear 140", 53.0, 53.0, 26.0, 26.0, 2.4));
            hublist.Add(new CHub("Campagnolo 1980's, 90's Chorus, C-Record, Athena low flange Front 100", 39.0, 39.0, 34.0, 34.0, 2.3));
            hublist.Add(new CHub("Campagnolo 1999+ Athena, Veloce, Mirage Front 100", 38.5, 38.5, 34.1, 34.1, 2.4));
            hublist.Add(new CHub("Campagnolo 1999+ Athena, Veloce, Mirage Rear 130", 44.0, 44.0, 36.5, 16.7, 2.4));
            hublist.Add(new CHub("Campagnolo 1999+ Avanti Front 100", 40.0, 40.0, 33.6, 33.6, 2.7));
            hublist.Add(new CHub("Campagnolo 1999+ Avanti Rear 130", 44.0, 44.0, 36.5, 16.7, 2.4));
            hublist.Add(new CHub("Campagnolo 8-speed cassette Rear 130", 44.5, 44.5, 36.0, 17.0, 2.4));
            hublist.Add(new CHub("Campagnolo all 120mm hi-flange models  Rear 120", 67.0, 67.0, 33.0, 24.0, 2.3));
            hublist.Add(new CHub("Campagnolo all 126mm hi-flange models  Rear 126", 67.0, 67.0, 36.0, 21.0, 2.3));
            hublist.Add(new CHub("Campagnolo all hi-flange models Front 100", 67.0, 67.0, 34.0, 34.0, 2.3));
            hublist.Add(new CHub("Campagnolo Athena, Chorus, Nuovo Record low flange Rear 127", 44.0, 44.0, 37.0, 20.0, 2.3));
            hublist.Add(new CHub("Campagnolo Centaur 8 spd cassette Rear 135", 44.5, 44.5, 35.5, 18.0, 2.4));
            hublist.Add(new CHub("Campagnolo Centaur, Euclid Rear 130", 44.5, 44.5, 36.0, 20.5, 2.4));
            hublist.Add(new CHub("Campagnolo Chorus, Daytona OS Front 100", 39.0, 39.0, 35.4, 35.4, 2.4));
            hublist.Add(new CHub("Campagnolo C-Record low flange Rear 126", 44.5, 44.5, 36.0, 21.0, 2.3));
            hublist.Add(new CHub("Campagnolo Hi-Lo Rear 120", 44.0, 67.0, 33.0, 24.0, 2.3));
            hublist.Add(new CHub("Campagnolo Nuovo Record low flange Front 100", 38.5, 38.5, 35.0, 35.0, 2.3));
            hublist.Add(new CHub("Campagnolo Record OS Front 100", 39.0, 39.0, 35.4, 35.4, 2.4));
            hublist.Add(new CHub("Campagnolo Record Rear 120", 44.0, 44.0, 34.0, 25.0, 2.7));
            hublist.Add(new CHub("Campagnolo Record, Chorus, Daytona OS Rear 130", 44.0, 46.0, 36.8, 16.8, 2.4));
            hublist.Add(new CHub("Campagnolo tandem  Front 100", 65.0, 65.0, 31.5, 31.5, 2.3));
            hublist.Add(new CHub("Campagnolo tandem cassette Rear 140", 65.0, 65.0, 28.0, 28.0, 2.4));
            hublist.Add(new CHub("Campagnolo track hi-flange C Record  Rear 120", 67.0, 67.0, 35.0, 30.0, 2.4));
            hublist.Add(new CHub("Campagnolo track hi-flange Nuovo Record  Rear ", 67.0, 67.0, 44.0, 31.0, 2.3));
            hublist.Add(new CHub("Campagnolo track low flange Rear 120", 44.0, 44.0, 44.5, 31.5, 2.3));
            hublist.Add(new CHub("Cannondale by DT Swiss Rear 130", 43.5, 43.5, 39.9, 14.5, 2.3));
            hublist.Add(new CHub("Cannondale by DT Swiss Rear 135", 43.5, 43.5, 37.4, 20.0, 2.3));
            hublist.Add(new CHub("Cannondale disc Rear 135", 57.0, 45.0, 35.0, 19.2, 2.5));
            hublist.Add(new CHub("Cannondale Lefty disc Front none", 58.0, 45.0, 18.35, 31.56, 2.5));
            hublist.Add(new CHub("Cannondale Moto120 Rear 135", 52.6, 52.6, 37.5, 16.6, 2.6));
            hublist.Add(new CHub("Chris King BMX Front 100", 40.0, 40.0, 34.0, 34.0, 2.3));
            hublist.Add(new CHub("Chris King cassette MTB Rear 135", 44.0, 53.0, 36.0, 21.0, 2.3));
            hublist.Add(new CHub("Chris King cassette road Rear 130", 44.0, 53.0, 38.5, 18.5, 2.3));
            hublist.Add(new CHub("Chris King Classic 140mm Rear 140", 44.0, 53.0, 33.5, 23.5, 2.3));
            hublist.Add(new CHub("Chris King Classic front narrow Front 100", 40.0, 40.0, 34.0, 34.0, 2.3));
            hublist.Add(new CHub("Chris King Classic front wide Front 100", 40.0, 40.0, 37.5, 37.5, 2.3));
            hublist.Add(new CHub("Chris King Classic mountain Rear 135", 44.0, 53.0, 36.0, 21.0, 2.3));
            hublist.Add(new CHub("Chris King Classic road/cross Rear 130", 44.0, 53.0, 38.5, 18.5, 2.3));
            hublist.Add(new CHub("Chris King Disc 20mm Front 110", 57.6, 57.6, 23.1, 31.2, 2.5));
            hublist.Add(new CHub("Chris King Disc ISO Front 100", 57.6, 57.6, 22.5, 30.4, 2.5));
            hublist.Add(new CHub("Chris King Disc ISO Rear 135", 57.6, 57.6, 33.9, 20.1, 2.5));
            hublist.Add(new CHub("Chris King Disc Single Speed Rear 135", 53.0, 53.0, 33.7, 33.7, 2.5));
            hublist.Add(new CHub("Chris King Disc Single Speed Rear 135", 53.0, 53.0, 34.2, 32.0, 2.5));
            hublist.Add(new CHub("Chris King Disc Universal 135mm Rear 135", 53.0, 53.0, 34.0, 21.0, 2.3));
            hublist.Add(new CHub("Chris King Disc Universal 140mm Rear 140", 53.0, 53.0, 31.5, 23.5, 2.3));
            hublist.Add(new CHub("Chris King Disc Universal 145mm Rear 145", 53.0, 53.0, 29.0, 26.0, 2.3));
            hublist.Add(new CHub("Chris King Disc Universal Front 100", 53.0, 53.0, 23.0, 31.5, 2.4));
            hublist.Add(new CHub("Chris King MTB Front 100", 40.0, 40.0, 37.5, 37.5, 2.3));
            hublist.Add(new CHub("Chris King Rear BMX Rear 110", 44.0, 53.0, 30.0, 27.0, 2.3));
            hublist.Add(new CHub("Chris King Rear DiscGoTech Rear 135", 53.0, 53.0, 34.0, 21.0, 2.3));
            hublist.Add(new CHub("Chris King Rear DiscGoTech Rear 140", 53.0, 53.0, 31.5, 23.5, 2.3));
            hublist.Add(new CHub("Chris King Rear DiscGoTech Rear 140", 53.0, 53.0, 31.5, 23.5, 2.5));
            hublist.Add(new CHub("Chris King Road Front 100", 40.0, 40.0, 34.0, 34.0, 2.3));
            hublist.Add(new CHub("Chris King Single Speed Front 100", 53.0, 53.0, 33.7, 33.7, 2.3));
            hublist.Add(new CHub("Chris King Single Speed Rear 135", 53.0, 53.0, 33.7, 33.7, 2.3));
            hublist.Add(new CHub("Chris King Tandem Front 100", 53.0, 53.0, 33.7, 33.7, 2.3));
            hublist.Add(new CHub("Chris King Tandem Rear 145", 53.0, 53.0, 29.0, 26.0, 2.3));
            hublist.Add(new CHub("Chris King Tandem Rear 160", 53.0, 53.0, 33.7, 33.7, 2.3));
            hublist.Add(new CHub("CODA  Front 100", 39.0, 39.0, 37.0, 37.0, 2.4));
            hublist.Add(new CHub("Coda 900 disc Rear 135", 58.0, 58.0, 37.0, 20.0, 2.6));
            hublist.Add(new CHub("Coda 901 disc Front 100", 58.0, 44.5, 20.0, 35.0, 2.6));
            hublist.Add(new CHub("Coda disc Front 100", 52.0, 40.0, 18.0, 38.0, 2.5));
            hublist.Add(new CHub("Coda DT Hugi 98 disc Rear 135", 52.7, 52.7, 36.0, 19.0, 2.5));
            hublist.Add(new CHub("Coda Expert disc Front 100", 58.0, 45.0, 19.0, 30.0, 2.6));
            hublist.Add(new CHub("Coda Expert disc Rear 135", 58.0, 44.5, 35.0, 20.0, 2.6));
            hublist.Add(new CHub("Coda Expert Non-disc Front 100", 44.5, 44.5, 31.0, 31.0, 2.6));
            hublist.Add(new CHub("Coda Expert non-disc Rear 135", 44.5, 44.5, 35.5, 18.5, 2.6));
            hublist.Add(new CHub("Coda Expert road Rear 130", 44.5, 44.5, 38.5, 14.5, 2.6));
            hublist.Add(new CHub("Coda Moto disc Front 100", 56.0, 62.0, 25.0, 31.0, 2.6));
            hublist.Add(new CHub("CORBIN DUPLEX coaster brake hub model 8 Rear ", 63.5, 54.0, 31.8, 27.0, 2.6));
            hublist.Add(new CHub("Corratec Carbon disc Front 100", 51.0, 39.5, 19.0, 34.0, 2.6));
            hublist.Add(new CHub("Corratec Carbon disc Rear 135", 51.0, 42.0, 30.0, 17.0, 2.6));
            hublist.Add(new CHub("Corratec Jumper Front 100", 55.0, 55.0, 35.0, 35.0, 2.6));
            hublist.Add(new CHub("Corratec Jumper Rear 135", 55.0, 45.0, 35.0, 20.0, 2.6));
            hublist.Add(new CHub("DAHON大行SP8 K标前花鼓 Front 74", 29.0, 29.0, 19.0, 19.0, 2.6));
            hublist.Add(new CHub("Danny's 12mm through axle Rear 165", 58.0, 58.0, 34.5, 34.5, 2.6));
            hublist.Add(new CHub("Decathlon by DT Swiss Front 100", 36.0, 36.0, 35.0, 35.0, 2.5));
            hublist.Add(new CHub("Decathlon by DT Swiss Rear 130", 45.0, 45.0, 37.7, 16.7, 2.5));
            hublist.Add(new CHub("Decathlon by DT Swiss Rear 135", 45.0, 45.0, 35.2, 19.2, 2.5));
            hublist.Add(new CHub("Dengler Front 100", 38.0, 38.0, 35.0, 35.0, 2.6));
            hublist.Add(new CHub("Dia-Compe Tsali  Front 100", 39.0, 39.0, 37.2, 37.2, 2.4));
            hublist.Add(new CHub("DMR Revolver Front 100", 41.0, 41.0, 36.0, 36.0, 2.4));
            hublist.Add(new CHub("DT Cerit disc Rear 135", 58.0, 52.0, 33.3, 20.0, 2.5));
            hublist.Add(new CHub("DT Cerit Front 100", 58.0, 52.0, 23.0, 34.7, 2.5));
            hublist.Add(new CHub("DT Hugi 240 Campy 9,10s Rear 130", 45.0, 45.0, 29.8, 16.1, 2.4));
            hublist.Add(new CHub("DT Hugi 240 disc Front 100", 58.0, 46.0, 22.0, 34.8, 2.5));
            hublist.Add(new CHub("DT Hugi 240 disc Rear 135", 58.0, 46.0, 33.5, 19.2, 2.5));
            hublist.Add(new CHub("DT Hugi 240 FR 12mm through axle Rear 150", 58.5, 58.5, 25.8, 25.8, 2.6));
            hublist.Add(new CHub("DT Hugi 240 FR 12mm through axle Rear 165", 58.5, 58.5, 33.3, 33.3, 2.6));
            hublist.Add(new CHub("DT Hugi 240 FR Rear 135", 58.0, 46.5, 33.5, 19.2, 2.6));
            hublist.Add(new CHub("DT Hugi 240 Front 100", 36.0, 36.0, 35.0, 35.0, 2.4));
            hublist.Add(new CHub("DT Hugi 240 Rear 130", 45.0, 45.0, 37.7, 16.7, 2.4));
            hublist.Add(new CHub("DT Hugi 240 Rear 135", 45.0, 45.0, 35.2, 19.2, 2.4));
            hublist.Add(new CHub("DT Hugi 240 road Rear 130", 45.0, 45.0, 29.8, 16.1, 2.4));
            hublist.Add(new CHub("DT Hugi 95 MTB Front 100", 41.0, 41.0, 35.0, 35.0, 2.3));
            hublist.Add(new CHub("DT Hugi 95 Rear 130", 45.5, 45.5, 39.5, 18.0, 2.3));
            hublist.Add(new CHub("DT Hugi 95 Rear 135", 45.5, 45.5, 37.4, 20.1, 2.3));
            hublist.Add(new CHub("DT Hugi 95 road Front 100", 39.0, 39.0, 34.5, 34.5, 2.3));
            hublist.Add(new CHub("DT Hugi 98 Campy 9s Rear 130", 46.0, 46.0, 33.7, 16.4, 2.3));
            hublist.Add(new CHub("DT Hugi 98 Front 100", 41.0, 41.0, 35.0, 35.0, 2.3));
            hublist.Add(new CHub("DT Hugi 98 Rear 130", 45.5, 45.5, 39.9, 17.6, 2.3));
            hublist.Add(new CHub("DT Hugi 98 Rear 135", 45.5, 45.5, 37.3, 20.2, 2.3));
            hublist.Add(new CHub("DT Hugi Formula disc Rear 135", 52.5, 46.0, 33.4, 19.7, 2.5));
            hublist.Add(new CHub("DT Hugi Formula Front 100", 52.5, 46.0, 17.0, 35.1, 2.5));
            hublist.Add(new CHub("DT Hugi FR disc Front 100", 58.0, 52.0, 22.0, 30.2, 2.6));
            hublist.Add(new CHub("DT Hugi FR disc Front 110", 58.0, 52.0, 22.0, 30.2, 2.6));
            hublist.Add(new CHub("DT Hugi Gustav M 98 disc DH Front 110", 60.0, 61.0, 26.6, 29.7, 2.8));
            hublist.Add(new CHub("DT Hugi Gustav M 98 disc Front 100", 60.0, 61.0, 23.7, 34.9, 2.8));
            hublist.Add(new CHub("DT Hugi Gustav M 98 Rear 135", 60.0, 61.0, 36.3, 19.0, 2.8));
            hublist.Add(new CHub("DT Hugi Gustav M 98 Rear 140", 60.0, 61.0, 33.0, 21.0, 2.8));
            hublist.Add(new CHub("DT Hugi Hayes DH Front 110", 58.0, 58.0, 22.8, 31.5, 2.5));
            hublist.Add(new CHub("DT Hugi Hayes disc Rear 135", 58.0, 46.0, 33.3, 19.8, 2.5));
            hublist.Add(new CHub("DT Hugi Hayes Front 100", 58.0, 46.0, 23.1, 34.4, 2.5));
            hublist.Add(new CHub("DT Hugi Louise 99 disc Front 100", 58.0, 47.0, 23.9, 35.6, 2.5));
            hublist.Add(new CHub("DT Hugi Louise 99 Rear 135", 58.0, 47.0, 39.0, 19.7, 2.5));
            hublist.Add(new CHub("DT Hugi Magura Disc Gustav M Rear 135", 60.0, 61.0, 36.3, 19.0, 2.8));
            hublist.Add(new CHub("DT Hugi Magura Disc Louise Rear 135", 58.0, 47.0, 39.0, 19.7, 2.5));
            hublist.Add(new CHub("DT Hugi MTB Rear 135", 45.0, 45.0, 35.0, 20.2, 2.3));
            hublist.Add(new CHub("DT Hugi road Campagnolo compatible Rear 130", 46.0, 46.0, 33.7, 16.4, 2.3));
            hublist.Add(new CHub("DT Hugi road Shimano compatible Rear 130", 45.0, 45.0, 39.9, 17.6, 2.3));
            hublist.Add(new CHub("DT Hugi Sport Front 100", 41.0, 41.0, 35.0, 35.0, 2.3));
            hublist.Add(new CHub("DT Hugi Sport Rear 130", 45.5, 45.5, 39.5, 18.0, 2.3));
            hublist.Add(new CHub("DT Hugi Sport Rear 135", 45.5, 45.5, 37.4, 20.1, 2.3));
            hublist.Add(new CHub("DT Hugi Tandem Arai Rear 135", 60.0, 60.0, 34.3, 19.5, 2.8));
            hublist.Add(new CHub("DT Hugi Tandem Arai Rear 140", 60.0, 60.0, 31.3, 21.5, 2.8));
            hublist.Add(new CHub("DT Hugi Tandem Arai Rear 145", 60.0, 60.0, 28.8, 24.0, 2.8));
            hublist.Add(new CHub("DT Hugi Tandem disc Front 100", 60.0, 61.0, 22.8, 34.4, 2.8));
            hublist.Add(new CHub("DT Hugi Tandem disc Rear 140", 60.0, 60.0, 31.3, 21.5, 2.8));
            hublist.Add(new CHub("DT Hugi Tandem disc Rear 145", 60.0, 60.0, 28.8, 24.0, 2.8));
            hublist.Add(new CHub("DT Hugi Tandem Front 100", 60.0, 60.0, 34.5, 34.5, 2.8));
            hublist.Add(new CHub("DT Onyx disc Front 100", 58.0, 48.0, 23.0, 34.7, 2.5));
            hublist.Add(new CHub("DT Onyx disc Front 110", 58.0, 58.0, 22.8, 31.5, 2.5));
            hublist.Add(new CHub("DT Onyx disc Rear 135", 58.0, 47.0, 33.3, 20.0, 2.5));
            hublist.Add(new CHub("DT Onyx Light Front 100", 40.0, 40.0, 34.2, 34.2, 2.5));
            hublist.Add(new CHub("DT Onyx Normal Front 100", 40.0, 40.0, 35.4, 35.4, 2.5));
            hublist.Add(new CHub("DT Swiss Onyx Rear 130", 46.0, 46.0, 32.1, 17.5, 2.5));
            hublist.Add(new CHub("DT Swiss Onyx Rear 135", 46.0, 46.0, 29.6, 20.0, 2.5));
            hublist.Add(new CHub("Edco Front 100", 41.0, 41.0, 35.0, 35.0, 2.5));
            hublist.Add(new CHub("Edco Rear 135", 45.0, 45.0, 34.0, 19.0, 2.5));
            hublist.Add(new CHub("EMAG hub motor Front 100", 178.0, 178.0, 20.0, 20.0, 2.5));
            hublist.Add(new CHub("Fimoco Front 100", 42.0, 42.0, 33.0, 33.0, 2.6));
            hublist.Add(new CHub("Formula by DT Swiss Front 100", 52.5, 46.0, 19.7, 35.1, 2.5));
            hublist.Add(new CHub("Formula by DT Swiss Rear 135", 52.5, 46.0, 33.4, 19.7, 2.5));
            hublist.Add(new CHub("Formula by Edco Front 100", 51.0, 51.0, 18.0, 34.0, 2.6));
            hublist.Add(new CHub("Formula by Edco Rear 135", 51.0, 51.0, 32.0, 20.0, 2.6));
            hublist.Add(new CHub("Formula DH Front 110", 45.0, 45.0, 22.0, 33.0, 2.6));
            hublist.Add(new CHub("Formula disc Front 100", 48.0, 48.0, 19.0, 38.0, 2.6));
            hublist.Add(new CHub("Formula Engineering SP92, road straight pull spokes. Front 100", 33.0, 33.0, 38.5, 38.5, 0.0));
            hublist.Add(new CHub("Formula Rear 135", 48.0, 48.0, 33.0, 21.0, 2.6));
            hublist.Add(new CHub("FORMULA尊轮 RB-52精磨滚珠花鼓 Rear 130", 45.0, 45.0, 34.0, 17.0, 3.0));
            hublist.Add(new CHub("FRM \"Lefty\" FL-M TEAM DISC (MTB) Front -", 59.0, 59.0, 22.9, 33.7, 2.4));
            hublist.Add(new CHub("FRM Comp & Pro Front 100", 34.0, 34.0, 34.5, 34.5, 2.4));
            hublist.Add(new CHub("FRM Comp & Pro Rear 130", 37.5, 41.0, 34.0, 18.5, 2.4));
            hublist.Add(new CHub("FRM Comp & Pro Rear 135", 37.5, 41.0, 36.5, 21.0, 2.4));
            hublist.Add(new CHub("FRM Feather Front Front 100", 35.0, 35.0, 34.5, 34.5, 2.4));
            hublist.Add(new CHub("FRM FL Disc Comp & Pro Front 100", 44.0, 38.0, 23.0, 33.4, 2.4));
            hublist.Add(new CHub("FRM FL Disc Comp & Pro Rear 135", 45.0, 45.0, 34.4, 21.0, 2.4));
            hublist.Add(new CHub("FRM FL-M PRO (MTB) Rear 135", 38.0, 45.0, 56.3, 21.1, 2.4));
            hublist.Add(new CHub("FRM FL-M PRO DISC Front 100", 44.0, 38.0, 22.8, 33.7, 2.4));
            hublist.Add(new CHub("FRM FL-M PRO DISC Rear 135", 45.0, 45.0, 33.9, 20.4, 2.4));
            hublist.Add(new CHub("FRM FL-M TEAM (MTB) Front 100", 59.0, 59.0, 28.3, 28.3, 2.4));
            hublist.Add(new CHub("FRM FL-M TEAM (MTB) Rear 135", 59.0, 59.0, 39.4, 17.2, 2.4));
            hublist.Add(new CHub("FRM FL-M TEAM DISC (MTB) Front 100", 59.0, 59.0, 22.9, 33.7, 2.4));
            hublist.Add(new CHub("FRM FL-M TEAM DISC (MTB) Rear 135", 59.0, 59.0, 39.4, 17.2, 2.4));
            hublist.Add(new CHub("FRM FL-R PRO & FL-M PRO (road & MTB) Front 100", 38.0, 38.0, 34.4, 34.4, 2.4));
            hublist.Add(new CHub("FRM FL-R PRO (road only) Rear 130", 38.0, 45.0, 37.7, 18.6, 2.4));
            hublist.Add(new CHub("FRM FL-R TEAM CA (road only) Front 100", 35.0, 35.0, 34.5, 34.5, 2.4));
            hublist.Add(new CHub("FRM FL-R TEAM CA (road only) Rear 130", 36.0, 42.0, 29.0, 18.0, 2.4));
            hublist.Add(new CHub("FRM Team SL Rear Rear 130", 37.5, 41.0, 34.0, 18.5, 2.4));
            hublist.Add(new CHub("Giant disc Front 100", 58.0, 54.0, 19.0, 34.0, 2.6));
            hublist.Add(new CHub("Giant disc Rear 135", 58.0, 54.0, 33.0, 20.0, 2.6));
            hublist.Add(new CHub("Gipiemme Special (Campy low flange copy) Front 100", 40.2, 40.2, 35.1, 35.1, 2.3));
            hublist.Add(new CHub("Gipiemme Special (Campy low flange copy) Rear 127", 44.5, 44.5, 36.8, 20.3, 2.3));
            hublist.Add(new CHub("Gipiemme Special (respaced from road to track) Rear 120", 44.5, 44.5, 29.5, 27.9, 2.3));
            hublist.Add(new CHub("Goldtec Track Hub Rear 135", 66.0, 66.0, 36.0, 36.0, 2.4));
            hublist.Add(new CHub("Goldtec-Ti Rear 135", 45.0, 45.0, 34.0, 18.0, 2.6));
            hublist.Add(new CHub("GT Front 100", 38.0, 38.0, 36.0, 36.0, 2.6));
            hublist.Add(new CHub("GT Rear 135", 45.0, 45.0, 35.0, 22.0, 2.6));
            hublist.Add(new CHub("Hadley 12mm through axle Rear 135", 60.0, 60.0, 34.0, 20.0, 2.6));
            hublist.Add(new CHub("Hadley Arai Rear 160", 56.0, 56.0, 30.0, 30.0, 2.6));
            hublist.Add(new CHub("Hadley H500 DH Front 110", 60.0, 60.0, 22.6, 31.4, 2.6));
            hublist.Add(new CHub("Hadley Santa Cruz 14mm through axle Rear 160", 60.0, 60.0, 33.5, 33.5, 2.6));
            hublist.Add(new CHub("Hanebrink LT 8 disc Front ", 55.0, 55.0, 17.0, 43.0, 2.8));
            hublist.Add(new CHub("Harris Cyclery High-flange track hub Front 100", 62.0, 62.0, 34.5, 34.5, 2.6));
            hublist.Add(new CHub("Harris Cyclery High-flange track hub Rear 120", 62.0, 62.0, 31.0, 31.0, 2.6));
            hublist.Add(new CHub("Hayes by EDCO Rear 135", 56.0, 56.0, 32.5, 17.5, 2.5));
            hublist.Add(new CHub("Hayes Elite disc DH Front 110", 58.0, 58.0, 22.8, 31.5, 2.5));
            hublist.Add(new CHub("Hayes Elite disc Front 100", 58.0, 48.0, 23.0, 34.7, 2.5));
            hublist.Add(new CHub("Hayes Elite disc Rear 135", 58.0, 47.0, 33.3, 20.0, 2.5));
            hublist.Add(new CHub("Hayes Superlight disc Front 100", 58.0, 46.0, 22.0, 34.8, 2.5));
            hublist.Add(new CHub("Hayes Superlight disc Rear 135", 58.0, 46.0, 33.5, 19.2, 2.5));
            hublist.Add(new CHub("Hershey Naked  Front 100", 31.5, 31.5, 32.0, 32.0, 2.4));
            hublist.Add(new CHub("Hershey Standard Ti  Front 100", 31.5, 31.5, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Hershey Suspension  Front 100", 44.5, 44.5, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Hershey Ti  Rear 135", 36.5, 48.0, 33.0, 21.0, 2.4));
            hublist.Add(new CHub("Heylight disc Front 100", 46.0, 33.0, 24.0, 26.0, 2.6));
            hublist.Add(new CHub("Heylight disc Rear 135", 46.0, 44.0, 39.0, 19.0, 2.6));
            hublist.Add(new CHub("Heylight Front 100", 33.0, 33.0, 32.0, 32.0, 2.6));
            hublist.Add(new CHub("Heylight Mini Front 100", 29.0, 29.0, 34.0, 34.0, 2.6));
            hublist.Add(new CHub("Heylight Rear 135", 45.0, 44.0, 33.0, 20.0, 2.6));
            hublist.Add(new CHub("Hi-E cassette Hi-Lo flange, custom narrow spacing Rear 130", 34.7, 54.0, 32.8, 19.0, 3.0));
            hublist.Add(new CHub("Hope Big'Un Front 100", 62.0, 62.0, 20.0, 25.0, 2.6));
            hublist.Add(new CHub("Hope Big'Un Rear 135", 62.0, 62.0, 25.0, 20.0, 2.6));
            hublist.Add(new CHub("Hope Bulb Front 100", 46.0, 46.0, 21.0, 34.0, 2.6));
            hublist.Add(new CHub("Hope Bulb IS2000 Front 100", 56.0, 46.7, 21.0, 33.0, 2.6));
            hublist.Add(new CHub("Hope Bulb IS2000 Rear 135", 56.0, 49.0, 33.0, 19.0, 2.6));
            hublist.Add(new CHub("Hope Bulb Rear 135", 44.0, 50.0, 33.0, 20.0, 2.6));
            hublist.Add(new CHub("Hope disc Front 100", 42.0, 42.0, 22.0, 32.0, 2.6));
            hublist.Add(new CHub("Hope disc Rear 135", 42.0, 45.0, 33.0, 20.0, 2.6));
            hublist.Add(new CHub("Hope Fatso  Front 100", 41.0, 41.0, 34.5, 34.5, 2.4));
            hublist.Add(new CHub("Hope Sport disc Front 100", 56.0, 45.0, 20.0, 34.0, 2.6));
            hublist.Add(new CHub("Hope Sport disc Rear 135", 45.0, 45.0, 33.0, 20.0, 2.6));
            hublist.Add(new CHub("Hope Super Ultralight  Front 100", 25.0, 25.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Hope Suspension  Front 100", 43.0, 43.0, 32.0, 32.0, 2.4));
            hublist.Add(new CHub("Hope tandem  Front 100", 65.0, 65.0, 32.5, 32.5, 2.4));
            hublist.Add(new CHub("Hope tandem  Rear 140", 65.0, 65.0, 27.5, 22.5, 2.4));
            hublist.Add(new CHub("Hope Tech  Front 100", 43.0, 43.0, 32.0, 32.0, 2.4));
            hublist.Add(new CHub("Hope Tech, narrower flanges Front 100", 43.0, 43.0, 22.5, 22.5, 2.4));
            hublist.Add(new CHub("Hope Tech, Ultralight Front 100", 34.0, 34.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Hope Ti-Glide disc Front 100", 43.0, 41.0, 22.0, 32.5, 2.6));
            hublist.Add(new CHub("Hope Ti-Glide disc Rear 135", 43.0, 44.5, 33.0, 20.0, 2.6));
            hublist.Add(new CHub("Hugi  Rear ", 45.0, 45.0, 37.5, 20.0, 2.4));
            hublist.Add(new CHub("Hugi 240 Front 100", 36.0, 36.0, 35.0, 35.0, 2.4));
            hublist.Add(new CHub("Hugi Formula Disc Front 100", 52.5, 46.0, 17.0, 35.1, 2.5));
            hublist.Add(new CHub("Hugi Hayes Disc Front 100", 58.0, 46.0, 23.7, 35.1, 2.5));
            hublist.Add(new CHub("Hugi Magura Disc Gustav M downhill Front 110", 60.0, 61.0, 26.6, 29.7, 2.8));
            hublist.Add(new CHub("Hugi Magura Disc Gustav M Front 100", 60.0, 61.0, 23.7, 34.9, 2.8));
            hublist.Add(new CHub("Hugi Magura Disc Louise Front 100", 58.0, 47.0, 23.9, 35.6, 2.5));
            hublist.Add(new CHub("Hugi road  Rear 130", 45.0, 45.0, 39.5, 18.0, 2.4));
            hublist.Add(new CHub("Hugi road  Rear 135", 45.0, 45.0, 37.5, 20.0, 2.4));
            hublist.Add(new CHub("Hugi tandem  Front 100", 41.0, 41.0, 35.0, 35.0, 2.4));
            hublist.Add(new CHub("Hugi tandem  Rear 140", 55.0, 55.0, 33.5, 24.0, 2.4));
            hublist.Add(new CHub("Hugi tandem large flange Front 100", 60.0, 60.0, 34.5, 34.5, 2.8));
            hublist.Add(new CHub("Hugi tandem Rear ", 55.0, 55.0, 37.0, 22.0, 2.3));
            hublist.Add(new CHub("Hugi TD (tandem) 2001 Front 100", 60.0, 60.0, 34.5, 34.5, 2.8));
            hublist.Add(new CHub("Hugi TD (tandem) 2001 Rear 140", 60.0, 60.0, 31.3, 21.5, 2.8));
            hublist.Add(new CHub("Hugi TD (tandem) 2001 Rear 145", 60.0, 60.0, 28.8, 24.0, 2.8));
            hublist.Add(new CHub("Hugi TD disc brake (tandem) 2001 Front 100", 60.0, 61.0, 21.6, 34.5, 2.8));
            hublist.Add(new CHub("Hugi/Union Std. Front 100", 39.0, 39.0, 33.0, 33.0, 2.5));
            hublist.Add(new CHub("Hugi/Union/Marwi Susp. Front 100", 41.0, 41.0, 35.0, 35.0, 2.5));
            hublist.Add(new CHub("Inferno disc Front 100", 5538.0, 55.8, 21.0, 33.0, 2.6));
            hublist.Add(new CHub("Inferno disc Rear 135", 55.8, 55.8, 35.0, 21.0, 2.6));
            hublist.Add(new CHub("Intense 20mm disc Front 110", 60.0, 60.0, 24.0, 31.0, 2.6));
            hublist.Add(new CHub("IRO High-flange track hub Front 100", 62.0, 62.0, 34.5, 34.5, 2.6));
            hublist.Add(new CHub("IRO High-flange track hub Rear 120", 62.0, 62.0, 31.0, 31.0, 2.6));
            hublist.Add(new CHub("IRO Single Speed disc brake hub Rear 135", 58.0, 58.0, 38.0, 34.0, 2.6));
            hublist.Add(new CHub("IRO Single Speed hub Front 100", 40.0, 40.0, 36.0, 36.0, 2.6));
            hublist.Add(new CHub("IRO Single Speed hub Rear 135", 53.0, 53.0, 39.0, 39.0, 2.6));
            hublist.Add(new CHub("Joytech sealed med flange Front 100", 44.0, 44.0, 28.5, 28.5, 2.4));
            hublist.Add(new CHub("Kingsbery low flange  Front 100", 42.5, 42.5, 34.5, 34.5, 2.4));
            hublist.Add(new CHub("Kingsbery low flange 135mm Rear 135", 42.5, 42.5, 34.5, 23.5, 2.4));
            hublist.Add(new CHub("Kogswell double fixed mtb/singlespeed rear Rear 135", 52.0, 52.0, 38.5, 38.5, 2.5));
            hublist.Add(new CHub("Kogswell front Front 100", 40.0, 40.0, 36.1, 36.1, 2.5));
            hublist.Add(new CHub("M bits Front 110", 58.5, 51.0, 30.0, 37.3, 2.5));
            hublist.Add(new CHub("Machine Tech Hollowcore  Front 100", 43.0, 43.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Machine Tech Power Claw  Rear 135", 43.0, 43.0, 34.0, 20.5, 2.4));
            hublist.Add(new CHub("Magura 1999 Louise disc Rear 135", 62.5, 62.5, 30.4, 20.8, 2.5));
            hublist.Add(new CHub("Magura Comp 2000 DH Front 110", 58.0, 58.0, 22.8, 31.5, 2.5));
            hublist.Add(new CHub("Magura Comp 2000 Front 100", 58.0, 48.0, 23.0, 34.7, 2.5));
            hublist.Add(new CHub("Magura Comp 2000 Rear 135", 58.0, 47.0, 33.3, 20.0, 2.5));
            hublist.Add(new CHub("Magura Fun Front 100", 58.0, 52.0, 23.0, 34.7, 2.5));
            hublist.Add(new CHub("Magura Fun Rear 135", 58.0, 52.0, 33.3, 20.0, 2.5));
            hublist.Add(new CHub("Magura Gustav M 2000 DH Front 110", 58.0, 58.0, 22.8, 31.5, 2.5));
            hublist.Add(new CHub("Magura Gustav M 2000 Front 100", 58.0, 46.0, 23.1, 34.4, 2.5));
            hublist.Add(new CHub("Magura Gustav M 2000 Rear 135", 58.0, 46.0, 33.3, 19.8, 2.5));
            hublist.Add(new CHub("Magura Gustav M 99 DH Front 110", 60.0, 61.0, 26.6, 29.7, 2.8));
            hublist.Add(new CHub("Magura Gustav M 99 Front 100", 60.0, 61.0, 23.7, 34.9, 2.8));
            hublist.Add(new CHub("Magura Gustav M 99 Rear 135", 60.0, 61.0, 36.3, 19.0, 2.8));
            hublist.Add(new CHub("Magura Pro 2000 Front 100", 58.0, 46.0, 22.0, 34.8, 2.5));
            hublist.Add(new CHub("Magura Pro 2000 Rear 135", 58.0, 46.0, 33.5, 19.2, 2.5));
            hublist.Add(new CHub("Magura/SRAM Louise 99 disc Front 100", 62.0, 62.0, 20.0, 30.0, 2.8));
            hublist.Add(new CHub("Magura/SRAM louise 99 Rear 135", 62.0, 62.0, 31.0, 20.0, 2.8));
            hublist.Add(new CHub("Maillard Professional High Flange Front 99", 67.5, 67.5, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Maillard Professional High Flange Rear 122.4", 67.5, 67.5, 35.9, 20.9, 2.4));
            hublist.Add(new CHub("Manitou through axle, non-disc Front ", 40.0, 40.0, 40.0, 40.0, 2.6));
            hublist.Add(new CHub("Marzocchi QR 20 disc Front ", 58.0, 58.0, 22.0, 31.0, 2.6));
            hublist.Add(new CHub("Marzocchi QR 20 Front ", 56.0, 56.0, 36.0, 36.0, 2.6));
            hublist.Add(new CHub("Marzocchi QR 20+ disc Front ", 61.0, 61.0, 19.0, 33.0, 3.0));
            hublist.Add(new CHub("Maverick American hub Front 110", 58.0, 50.0, 29.4, 36.5, 2.5));
            hublist.Add(new CHub("Mavic 500, 501, 520, 530, 531, 550, 571, 577 Front 100", 40.0, 40.0, 27.0, 27.0, 2.5));
            hublist.Add(new CHub("Mavic 501  Rear 126", 44.5, 44.5, 30.0, 18.0, 2.4));
            hublist.Add(new CHub("Mavic 501  Rear 130", 44.5, 44.5, 31.5, 16.5, 2.4));
            hublist.Add(new CHub("Mavic 506  Rear 126", 44.5, 44.5, 37.0, 19.0, 2.4));
            hublist.Add(new CHub("Mavic 520 track low flange Rear 120", 44.5, 44.5, 42.5, 30.5, 2.4));
            hublist.Add(new CHub("Mavic 531 MTB  Rear 130", 44.5, 44.5, 29.5, 18.5, 2.4));
            hublist.Add(new CHub("Mavic 531 MTB  Rear 135", 44.5, 44.5, 27.0, 21.0, 2.4));
            hublist.Add(new CHub("Mavic 550 Rear 126", 45.0, 45.0, 37.7, 19.2, 2.4));
            hublist.Add(new CHub("Mavic 570 Rear 120", 45.0, 45.0, 36.0, 26.0, 2.4));
            hublist.Add(new CHub("Mavic 571 road  Rear 130", 53.0, 53.0, 32.0, 16.5, 2.4));
            hublist.Add(new CHub("Mavic 571, 572  Rear 130", 53.0, 53.0, 35.0, 19.0, 2.4));
            hublist.Add(new CHub("Mavic 577 MTB  Rear 135", 53.0, 53.0, 31.5, 17.0, 2.4));
            hublist.Add(new CHub("Mavic Cosmic Carbone 2001 blk. STRAIGHT SPOKES Front 100", 27.0, 27.0, 36.0, 36.0, 0.0));
            hublist.Add(new CHub("Mavic MR601 Front 100", 40.0, 40.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Mavic MR601 Rear 130", 46.0, 48.0, 34.0, 17.4, 2.4));
            hublist.Add(new CHub("Mavic special small TT hub Front 80", 34.0, 34.0, 28.0, 28.0, 2.4));
            hublist.Add(new CHub("Maxx Front 100", 32.0, 32.0, 35.0, 35.0, 2.6));
            hublist.Add(new CHub("Miche track Rear 120", 59.0, 59.0, 45.0, 30.0, 2.5));
            hublist.Add(new CHub("Nashbar Fixed-Fixed Flip-Flop front hub NF-GHF Front 100", 62.0, 62.0, 34.0, 34.0, 2.6));
            hublist.Add(new CHub("Nashbar Fixed-Fixed Flip-Flop rear hub NF-GHR Rear 120", 62.0, 62.0, 31.0, 31.0, 2.6));
            hublist.Add(new CHub("NC 17 disc DH Front 110", 60.0, 60.0, 21.5, 33.9, 2.8));
            hublist.Add(new CHub("NC 17 disc Front 100", 60.0, 60.0, 21.5, 33.9, 2.8));
            hublist.Add(new CHub("NC 17 disc Rear 135", 60.0, 60.0, 31.7, 18.2, 2.8));
            hublist.Add(new CHub("Nicolai Nucleon NC 17 95mm 20mm through axle Front ", 60.0, 60.0, 19.0, 36.0, 2.8));
            hublist.Add(new CHub("Normandy high flange Front 100", 63.0, 63.0, 33.7, 33.7, 2.4));
            hublist.Add(new CHub("Normandy high flange Rear 120", 63.0, 63.0, 39.0, 19.0, 2.4));
            hublist.Add(new CHub("Normandy high flange respaced to 126mm Rear 126", 63.0, 63.0, 42.0, 16.0, 2.4));
            hublist.Add(new CHub("NOVATECH久裕 A171SB Front 100", 38.0, 38.0, 36.0, 36.0, 3.0));
            hublist.Add(new CHub("NOVATECH久裕 A211SB Front 74", 30.0, 30.0, 22.0, 22.0, 3.0));
            hublist.Add(new CHub("NOVATECH久裕 A551SB Front 74", 30.0, 30.0, 22.0, 22.0, 3.0));
            hublist.Add(new CHub("NOVATECH久裕 D041SB Front 100", 58.0, 58.0, 34.6, 27.4, 2.5));
            hublist.Add(new CHub("NOVATECH久裕 D042SB Rear 135", 58.0, 58.0, 32.6, 25.8, 2.5));
            hublist.Add(new CHub("NOVATECH久裕 F062SB Rear 135", 49.0, 49.0, 36.5, 20.5, 3.0));
            hublist.Add(new CHub("NOVATECH久裕 F172SB Rear 130", 49.0, 49.0, 38.0, 19.0, 3.0));
            hublist.Add(new CHub("NOVATECH久裕 F192SB Rear 130", 45.0, 45.0, 31.1, 18.9, 2.6));
            hublist.Add(new CHub("Nuke Proof  Front BMX", 32.0, 32.0, 36.5, 36.5, 2.4));
            hublist.Add(new CHub("Nuke Proof  Rear 126", 32.0, 43.1, 45.0, 20.0, 2.4));
            hublist.Add(new CHub("Nuke Proof  Rear 130", 32.0, 43.1, 43.0, 22.0, 2.4));
            hublist.Add(new CHub("Nuke Proof  Rear 135", 32.0, 43.1, 41.0, 25.0, 2.4));
            hublist.Add(new CHub("Nuke Proof Bombshell cassette  Rear 130", 47.0, 47.0, 42.0, 19.5, 2.4));
            hublist.Add(new CHub("Nuke Proof Bombshell cassette  Rear 135", 47.0, 47.0, 41.0, 21.0, 2.4));
            hublist.Add(new CHub("Nuke Proof Bombshell, post '95 Front 100", 47.0, 47.0, 38.0, 38.0, 2.4));
            hublist.Add(new CHub("Nuke Proof Bombshell, pre '95 Front 100", 42.2, 42.2, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Nuke Proof Front 100", 31.0, 31.0, 35.3, 35.3, 2.6));
            hublist.Add(new CHub("Nuke Proof XT cassette  Rear 126", 32.0, 47.3, 45.0, 20.0, 2.4));
            hublist.Add(new CHub("Nuke Proof XT cassette  Rear 130", 32.0, 47.3, 43.0, 22.0, 2.4));
            hublist.Add(new CHub("Nuke Proof XT cassette  Rear 135", 32.0, 47.3, 40.0, 25.0, 2.4));
            hublist.Add(new CHub("Nuke Proof XTR cassette  Rear 130", 32.0, 47.5, 38.5, 20.0, 2.4));
            hublist.Add(new CHub("Nuke Proof XTR cassette  Rear 135", 32.0, 47.3, 38.5, 21.0, 2.4));
            hublist.Add(new CHub("Nuke Proof, older models Front 100", 32.0, 32.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("On-one 120mm Double Fixed Rear 120", 46.0, 46.0, 27.0, 27.0, 2.5));
            hublist.Add(new CHub("Pace carbon sealed  Front 100", 44.5, 44.5, 32.0, 32.0, 2.4));
            hublist.Add(new CHub("Pace carbon sealed  Rear 135", 44.5, 44.5, 38.0, 22.5, 2.4));
            hublist.Add(new CHub("Paul Components  Front 100", 43.0, 43.0, 36.0, 36.0, 2.4));
            hublist.Add(new CHub("Phil Wood Cassette Rear 130", 55.0, 55.0, 35.0, 22.0, 2.3));
            hublist.Add(new CHub("Phil Wood Cross Country  Front 100", 57.5, 57.5, 27.8, 27.8, 2.4));
            hublist.Add(new CHub("Phil Wood Field Serviceable  Front 100", 48.5, 48.5, 34.5, 34.5, 2.4));
            hublist.Add(new CHub("Phil Wood Field Serviceable  Rear 126", 48.5, 48.5, 37.6, 19.4, 2.4));
            hublist.Add(new CHub("Phil Wood Field Serviceable  Rear 130", 48.5, 48.5, 34.9, 22.1, 2.4));
            hublist.Add(new CHub("Phil Wood Field Serviceable  Rear 135", 48.5, 48.5, 32.4, 24.6, 2.4));
            hublist.Add(new CHub("Phil Wood Field Serviceable  Rear 140", 48.5, 48.5, 39.0, 18.0, 2.4));
            hublist.Add(new CHub("Phil Wood Hi right hand flange Rear 126", 48.5, 69.0, 38.0, 19.0, 2.4));
            hublist.Add(new CHub("Phil Wood Hi right hand flange Rear 130", 48.5, 69.0, 36.6, 20.4, 2.4));
            hublist.Add(new CHub("Phil Wood medium original  Front 100", 48.5, 48.5, 34.5, 34.5, 2.4));
            hublist.Add(new CHub("Phil Wood medium original  Rear 126", 48.5, 48.5, 38.0, 19.0, 2.4));
            hublist.Add(new CHub("Phil Wood medium original  Rear 140", 48.5, 48.5, 31.0, 26.0, 2.4));
            hublist.Add(new CHub("Phil Wood Mid  Rear 126", 48.5, 57.5, 38.0, 19.0, 2.4));
            hublist.Add(new CHub("Phil Wood Mid  Rear 130", 48.5, 57.5, 36.0, 21.0, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 6speed  Rear 125", 57.5, 48.2, 36.7, 20.3, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 6speed  Rear 130", 57.5, 48.2, 34.2, 22.8, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 6speed  Rear 135", 57.5, 48.2, 31.7, 25.3, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 7speed  Rear 126", 57.5, 48.2, 37.5, 19.5, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 7speed  Rear 130", 57.5, 48.2, 35.7, 21.3, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 7speed  Rear 135", 57.5, 48.2, 33.2, 23.8, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 7speed  Rear 140", 57.5, 48.2, 30.7, 26.3, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 8speed  Rear 130", 57.5, 48.2, 39.0, 18.0, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 8speed  Rear 135", 57.5, 48.2, 38.2, 18.8, 2.4));
            hublist.Add(new CHub("Phil Wood MTB 8speed  Rear 140", 58.5, 49.2, 35.7, 21.3, 2.4));
            hublist.Add(new CHub("Phil Wood road 5speed  Rear 120", 48.2, 48.2, 34.2, 22.8, 2.4));
            hublist.Add(new CHub("Phil Wood road 5speed  Rear 126", 48.2, 48.2, 31.7, 25.3, 2.4));
            hublist.Add(new CHub("Phil Wood road 6speed  Rear 126", 48.2, 48.2, 36.7, 20.3, 2.4));
            hublist.Add(new CHub("Phil Wood road 6speed  Rear 130", 48.2, 48.2, 34.2, 22.8, 2.4));
            hublist.Add(new CHub("Phil Wood road 6speed  Rear 135", 48.2, 48.2, 31.7, 25.3, 2.4));
            hublist.Add(new CHub("Phil Wood road 7speed  Rear 126", 48.2, 48.2, 37.2, 19.8, 2.4));
            hublist.Add(new CHub("Phil Wood road 7speed  Rear 130", 48.2, 48.2, 35.7, 21.3, 2.4));
            hublist.Add(new CHub("Phil Wood road 7speed  Rear 135", 48.2, 48.2, 33.2, 23.8, 2.4));
            hublist.Add(new CHub("Phil Wood road 8speed  Rear 130", 48.2, 48.2, 39.0, 18.0, 2.4));
            hublist.Add(new CHub("Phil Wood road 8speed  Rear 135", 48.2, 48.2, 38.2, 18.8, 2.4));
            hublist.Add(new CHub("Phil Wood road 8speed  Rear 140", 48.2, 48.2, 35.7, 21.3, 2.4));
            hublist.Add(new CHub("Phil Wood small old  Front 100", 42.5, 42.5, 35.0, 35.0, 2.4));
            hublist.Add(new CHub("Phil Wood small old  Rear 126", 42.5, 48.5, 38.0, 19.0, 2.4));
            hublist.Add(new CHub("Phil Wood small old  Rear 130", 42.5, 48.5, 36.0, 21.0, 2.4));
            hublist.Add(new CHub("Phil Wood small old  Rear 135", 42.5, 48.5, 33.5, 23.5, 2.4));
            hublist.Add(new CHub("Phil Wood Suspension  Front 100", 57.5, 57.5, 35.3, 35.3, 2.4));
            hublist.Add(new CHub("Phil Wood Time Trial  Front 100", 48.5, 48.5, 37.5, 37.5, 2.4));
            hublist.Add(new CHub("Phil Wood track double-sided, small flange Rear 120,126,130", 48.2, 48.2, 30.0, 30.0, 2.4));
            hublist.Add(new CHub("Phil Wood track front, small flange Front 100", 48.2, 48.2, 34.5, 34.5, 2.4));
            hublist.Add(new CHub("Phil Wood track one-sided, small flange Rear 120,126,130", 48.5, 48.5, 44.0, 30.0, 2.4));
            hublist.Add(new CHub("Phil Wood track, large flange Rear 120", 66.0, 66.0, 43.0, 29.0, 2.6));
            hublist.Add(new CHub("Phil Wood wheelchair Rear ", 41.0, 41.0, 29.5, 29.5, 2.4));
            hublist.Add(new CHub("Point HR Rear 135", 45.0, 45.0, 32.0, 21.0, 2.8));
            hublist.Add(new CHub("Power-tap ATB Rear 153", 78.0, 66.0, 30.5, 19.5, 2.4));
            hublist.Add(new CHub("Power-tap Rear 130", 78.0, 66.0, 36.2, 16.7, 2.4));
            hublist.Add(new CHub("Power-tap SL Rear 130", 70.0, 70.0, 31.8, 18.0, 2.5));
            hublist.Add(new CHub("powerway R13 Rear 130", 38.0, 49.0, 39.0, 15.0, 2.6));
            hublist.Add(new CHub("Pulstar 28 hole Front 100", 41.3, 41.3, 32.5, 32.5, 2.4));
            hublist.Add(new CHub("Pulstar 32 hole Front 100", 47.6, 47.6, 32.5, 32.5, 2.4));
            hublist.Add(new CHub("Pulstar 7-spd 32-hole  Rear ", 47.6, 47.6, 32.0, 22.0, 2.4));
            hublist.Add(new CHub("Pulstar 7-spd 36-hole Rear ", 50.8, 50.8, 32.0, 22.0, 2.4));
            hublist.Add(new CHub("Pulstar 8-spd 32-hole  Rear ", 47.6, 47.6, 34.0, 20.0, 2.4));
            hublist.Add(new CHub("Pulstar 8-spd 36-hole Rear ", 50.8, 50.8, 34.0, 20.0, 2.4));
            hublist.Add(new CHub("Pure Power Front 100", 48.0, 48.0, 35.0, 35.0, 2.6));
            hublist.Add(new CHub("Real front Front 100", 40.0, 40.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Real rear, not for disc brake Rear ", 37.0, 54.5, 37.0, 21.0, 2.4));
            hublist.Add(new CHub("Ringlé 6-spd MTB  Rear 130", 40.0, 41.0, 41.5, 22.5, 2.4));
            hublist.Add(new CHub("Ringlé 7-spd MTB  Rear 135", 40.0, 41.0, 38.5, 25.5, 2.4));
            hublist.Add(new CHub("Ringlé 7-spd road  Rear 126", 40.0, 41.0, 43.0, 21.0, 2.4));
            hublist.Add(new CHub("Ringlé 8-spd MTB  Rear 135", 40.0, 41.0, 39.5, 20.5, 2.4));
            hublist.Add(new CHub("Ringlé 8-spd road  Rear 130", 40.0, 41.0, 42.0, 18.0, 2.4));
            hublist.Add(new CHub("Ringlé Bubba  Front 100", 40.0, 40.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Ringlé Bubba HR freewheel Rear 135", 40.0, 41.0, 38.5, 25.5, 2.6));
            hublist.Add(new CHub("Ringlé Super Bubba  Front 100", 42.0, 42.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Ringlé Super Bubba Rear 135", 45.0, 45.0, 34.0, 22.0, 2.6));
            hublist.Add(new CHub("Ringlé Super Duper Bubba  Front 100", 46.0, 46.0, 34.0, 34.0, 2.6));
            hublist.Add(new CHub("Ringlé SuperDuper Eight MTB  Rear 135", 52.0, 52.0, 34.0, 21.0, 2.6));
            hublist.Add(new CHub("Ringlé SuperEight MTB  Rear 135", 45.0, 45.0, 34.0, 22.0, 2.4));
            hublist.Add(new CHub("Ringlé SuperEight road  Rear 130", 45.0, 45.0, 36.5, 19.5, 2.4));
            hublist.Add(new CHub("Ringlé/Sun \"Sun of Bubba\" Front 100", 38.0, 38.0, 32.0, 32.0, 2.4));
            hublist.Add(new CHub("Ritchey disc Pro Skraxle Front 100", 58.0, 58.0, 22.0, 35.0, 2.8));
            hublist.Add(new CHub("Ritchey Pro disc Rear 135", 58.0, 58.0, 35.0, 20.0, 2.8));
            hublist.Add(new CHub("Ritchey Z-hub MTB: WCS, Pro, Comp Front 100", 38.0, 38.0, 38.0, 38.0, 2.4));
            hublist.Add(new CHub("Ritchey Z-hub MTB: WCS, Pro, Comp Rear 130", 45.0, 45.0, 28.1, 21.9, 2.4));
            hublist.Add(new CHub("Ritchey Z-hub road: Pro Rear 130", 45.0, 45.0, 28.6, 21.3, 2.4));
            hublist.Add(new CHub("Ritchey Z-hub road: WCS Rear 130", 45.0, 45.0, 28.5, 21.5, 2.4));
            hublist.Add(new CHub("Ritchey Z-hub road: WCS, Pro Front 100", 38.0, 38.0, 36.0, 36.0, 2.4));
            hublist.Add(new CHub("Rocky Mountain disc QR20 Front 110", 62.0, 62.0, 24.0, 32.0, 2.5));
            hublist.Add(new CHub("Rohloff Speed Hub 500/14 Rear ", 100.0, 100.0, 30.0, 30.0, 2.5));
            hublist.Add(new CHub("Rolf ATB PN 210246 Urraco disc. Use 2.25 cross. Front 100", 50.0, 42.0, 23.3, 32.3, 2.5));
            hublist.Add(new CHub("Rolf ATB PN 210668 Dolomite disc. Use 2.25 cross. Front 100", 50.0, 42.0, 23.3, 32.3, 2.5));
            hublist.Add(new CHub("Rolf ATB PN 990034 Propel. Radial, heads out. Front 100", 42.0, 42.0, 32.8, 32.8, 2.3));
            hublist.Add(new CHub("Rolf Road PN 200322 Vector Pro. Radial, heads out. Front 100", 39.0, 39.0, 32.6, 32.6, 2.8));
            hublist.Add(new CHub("Rolf Road PN 200328 Vector Comp. Use 2.29 cross. Rear 130", 65.0, 47.0, 34.6, 17.8, 2.5));
            hublist.Add(new CHub("Rolf Road PN 200385 Vector Comp. Radial, heads out. Rear 130", 42.0, 42.0, 31.3, 31.3, 2.5));
            hublist.Add(new CHub("Rolf Road PN 210948 Vector. Use 2.25 cross. Rear 130", 65.0, 47.0, 34.6, 17.8, 2.3));
            hublist.Add(new CHub("Rolf Road PN 992473 Vector. Radial, heads out. Front 100", 38.0, 38.0, 34.1, 34.1, 2.5));
            hublist.Add(new CHub("Rolf Road Vector Pro. Use 1.333 cross. Rear 130", 65.0, 47.0, 33.0, 17.5, 2.5));
            hublist.Add(new CHub("Rolf Track Vector Pro. Use 1.333 cross. Rear 120", 66.0, 48.0, 39.0, 33.0, 2.5));
            hublist.Add(new CHub("Rollex Freecoaster BMX Rear 110", 67.0, 67.0, 31.0, 25.0, 2.6));
            hublist.Add(new CHub("Roox Jim Bob 99 DH Front 110", 61.0, 61.0, 25.0, 30.0, 2.8));
            hublist.Add(new CHub("Roox Jim Bob Rear 135", 61.0, 61.0, 35.0, 20.0, 2.8));
            hublist.Add(new CHub("Rotwild Schweizer Loop Inter-4 Rear 135", 83.0, 83.0, 28.5, 25.5, 2.8));
            hublist.Add(new CHub("Sachs 3000, 5000, New Success Front 100", 38.5, 38.5, 28.5, 28.5, 2.4));
            hublist.Add(new CHub("Sachs 3-speed coaster  Rear 118", 60.0, 60.0, 25.5, 24.5, 2.4));
            hublist.Add(new CHub("Sachs 3-speed drum brake  Rear 118", 89.0, 89.0, 30.5, 23.5, 2.4));
            hublist.Add(new CHub("Sachs 3-speed standard  Rear 110", 58.0, 58.0, 26.5, 24.0, 2.4));
            hublist.Add(new CHub("Sachs 3X7 Centera, Quarz  Rear 135", 67.0, 67.0, 35.0, 20.0, 2.4));
            hublist.Add(new CHub("Sachs 3x7 disc/non-disc Rear 135", 67.0, 67.0, 34.0, 20.0, 2.8));
            hublist.Add(new CHub("Sachs 3X7 Original  Rear 130", 67.0, 67.0, 37.0, 18.0, 2.4));
            hublist.Add(new CHub("Sachs 3X7 Traxx  Rear 135", 58.0, 58.0, 35.0, 20.0, 2.4));
            hublist.Add(new CHub("Sachs 7 speed cassette  Rear 135", 45.0, 45.0, 31.6, 19.6, 2.4));
            hublist.Add(new CHub("Sachs 8 speed cassette  Rear 135", 45.0, 45.0, 32.6, 18.6, 2.4));
            hublist.Add(new CHub("Sachs HT3020 drum brake  Rear ", 87.0, 87.0, 35.0, 17.0, 2.4));
            hublist.Add(new CHub("Sachs Inter 12 Rear ", 105.0, 131.0, 37.0, 23.0, 2.8));
            hublist.Add(new CHub("Sachs Orbit Drum 6-speed  Rear 126", 90.0, 90.0, 36.0, 19.0, 2.4));
            hublist.Add(new CHub("Sachs Orbit Drum 7-speed  Rear 130", 90.0, 90.0, 37.2, 17.8, 2.4));
            hublist.Add(new CHub("Sachs Orbit MTB 7-speed  Rear 130", 67.0, 54.0, 39.0, 18.0, 2.4));
            hublist.Add(new CHub("Sachs Orbit standard 6-speed  Rear 126", 67.0, 54.0, 38.0, 19.0, 2.4));
            hublist.Add(new CHub("Sachs Pentasport coaster  Rear 122", 75.0, 75.0, 29.5, 28.5, 2.4));
            hublist.Add(new CHub("Sachs Pentasport drum  Rear 126", 90.0, 90.0, 29.5, 30.5, 2.4));
            hublist.Add(new CHub("Sachs Pentasport Std.  Rear 122", 75.0, 75.0, 29.6, 29.0, 2.4));
            hublist.Add(new CHub("Sachs Quartz disc Rear 135", 62.0, 62.0, 35.0, 19.0, 2.8));
            hublist.Add(new CHub("Sachs Quarz, Neos disc brake hubs Front 100", 62.0, 62.0, 20.0, 30.0, 2.8));
            hublist.Add(new CHub("Sachs Quarz, Neos, Centera  Front 100", 39.0, 39.0, 29.6, 29.6, 2.4));
            hublist.Add(new CHub("Sachs Super 7 coaster  Rear 130", 75.0, 75.0, 35.5, 33.0, 2.4));
            hublist.Add(new CHub("Sachs Super 7 drum  Rear 135", 75.0, 75.0, 35.7, 34.8, 2.4));
            hublist.Add(new CHub("Sachs Super 7 standard  Rear 130", 75.0, 75.0, 34.5, 34.0, 2.4));
            hublist.Add(new CHub("Sachs threaded  Rear 126", 45.0, 45.0, 34.1, 17.1, 2.4));
            hublist.Add(new CHub("Sachs threaded  Rear 130", 45.0, 45.0, 34.6, 16.6, 2.4));
            hublist.Add(new CHub("Sachs Traxx  Front 100", 39.0, 39.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Sachs VT3000 drum brake  Front 100", 87.0, 87.0, 26.0, 26.0, 2.4));
            hublist.Add(new CHub("Sachs VT5000 drum brake  Front 100", 90.0, 90.0, 30.0, 26.0, 2.4));
            hublist.Add(new CHub("Sachs VT5000 drum brake  Rear ", 90.0, 90.0, 37.0, 19.0, 2.4));
            hublist.Add(new CHub("Sanshin low flange (like Specialized) Front 100", 38.0, 38.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Sanshin low flange (like Specialized) Rear 126", 44.5, 44.5, 35.5, 20.0, 2.4));
            hublist.Add(new CHub("Sanshin low flange (like Specialized) Rear 130", 44.5, 44.5, 33.5, 22.0, 2.4));
            hublist.Add(new CHub("Sanshin medium flange (like Specialized) Front 100", 43.0, 43.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Sanshin medium flange (like Specialized) Rear older", 46.0, 46.0, 28.0, 22.0, 2.4));
            hublist.Add(new CHub("Sanshin medium flange (like Specialized) Rear older", 46.0, 46.0, 33.0, 18.0, 2.4));
            hublist.Add(new CHub("Schwinn unicycle hub n/a n/a", 62.1, 62.1, 37.7, 37.7, 2.8));
            hublist.Add(new CHub("Shimano 105  HB-5501 Front 100", 38.0, 38.0, 38.7, 38.7, 2.4));
            hublist.Add(new CHub("Shimano 105 FH-5500 Rear 130", 45.0, 45.0, 32.5, 20.5, 2.6));
            hublist.Add(new CHub("Shimano 105SC FH-1055 7S Rear 126", 45.0, 45.0, 37.3, 20.7, 2.6));
            hublist.Add(new CHub("Shimano 105SC HB-1055-F Front 100", 38.0, 38.0, 36.3, 36.3, 2.6));
            hublist.Add(new CHub("Shimano 200GS FH-HG20-QR Rear 126", 45.0, 45.0, 38.2, 20.2, 2.6));
            hublist.Add(new CHub("Shimano 200GS FH-HG20-QR Rear 130", 45.0, 45.0, 36.2, 22.2, 2.6));
            hublist.Add(new CHub("Shimano 400CX FH-C040 Rear 130", 45.0, 45.0, 36.2, 22.2, 2.6));
            hublist.Add(new CHub("Shimano 400CX HB-C400 Front 100", 38.0, 38.0, 36.3, 36.3, 2.6));
            hublist.Add(new CHub("Shimano 600 FH-6400 6S Rear 126", 45.0, 45.0, 35.0, 19.0, 2.6));
            hublist.Add(new CHub("Shimano 600 Ultegra FH-6401 7S Rear 126", 45.0, 45.0, 37.3, 20.7, 2.6));
            hublist.Add(new CHub("Shimano 600 Ultegra FH-6402 8S Rear 130", 45.0, 45.0, 37.1, 20.9, 2.6));
            hublist.Add(new CHub("Shimano 600 Ultegra HB-6400 Front 100", 38.0, 38.0, 36.3, 36.3, 2.6));
            hublist.Add(new CHub("Shimano 700CX FH-C070 Rear 130", 45.0, 45.0, 35.3, 22.7, 2.6));
            hublist.Add(new CHub("Shimano 700CX HB-C700 Front 100", 38.0, 38.0, 36.3, 36.3, 2.6));
            hublist.Add(new CHub("Shimano Acera FH-M290 Rear 130", 45.0, 45.0, 38.1, 22.5, 2.6));
            hublist.Add(new CHub("Shimano Acera FH-M290 Rear 135", 45.0, 45.0, 35.6, 25.0, 2.6));
            hublist.Add(new CHub("Shimano Acera HB-M290 Front 100", 38.0, 38.0, 35.8, 35.8, 2.6));
            hublist.Add(new CHub("Shimano Alivio FH-MC18AZ Rear 135", 45.0, 45.0, 34.5, 21.5, 2.6));
            hublist.Add(new CHub("Shimano Alivio FH-R050 Rear 130", 45.0, 45.0, 38.1, 22.5, 2.6));
            hublist.Add(new CHub("Shimano Alivio FH-R050 Rear 135", 45.0, 45.0, 35.6, 25.0, 2.6));
            hublist.Add(new CHub("Shimano Altus C90 FH-CT90 Rear 126", 45.0, 45.0, 37.6, 23.0, 2.6));
            hublist.Add(new CHub("Shimano Altus C90 FH-CT90 Rear 130", 45.0, 45.0, 35.6, 25.0, 2.6));
            hublist.Add(new CHub("Shimano Altus C90 FH-CT90 Rear 130", 45.0, 45.0, 38.1, 22.5, 2.6));
            hublist.Add(new CHub("Shimano Altus C90 FH-CT90 Rear 135", 45.0, 45.0, 35.6, 25.0, 2.6));
            hublist.Add(new CHub("Shimano Altus C90 HB-CT90 Front 96 or 100", 38.0, 38.0, 35.8, 35.8, 2.6));
            hublist.Add(new CHub("Shimano Deore DX cassette Rear 130", 45.0, 45.0, 38.1, 22.5, 2.6));
            hublist.Add(new CHub("Shimano Deore HB-M510 Front 100", 42.0, 42.0, 34.5, 34.5, 2.6));
            hublist.Add(new CHub("Shimano Deore HB-M530 Front 100", 38.0, 38.0, 35.8, 35.8, 2.6));
            hublist.Add(new CHub("Shimano Deore LX & STX-RC. FH-R080  Rear 135", 45.0, 45.0, 37.9, 22.7, 2.6));
            hublist.Add(new CHub("Shimano Deore LX FH-563 Rear 130", 45.0, 45.0, 37.6, 23.0, 2.6));
            hublist.Add(new CHub("Shimano Deore LX FH-563 Rear 135", 45.0, 45.0, 35.1, 25.5, 2.6));
            hublist.Add(new CHub("Shimano Deore LX FH-565 Rear 135", 45.0, 45.0, 37.9, 22.7, 2.6));
            hublist.Add(new CHub("Shimano Deore LX FH-570 Rear 135", 45.0, 45.0, 36.8, 23.2, 2.6));
            hublist.Add(new CHub("Shimano Deore LX HB-M563, HB-M564, HB-M570 Front 100", 38.0, 38.0, 35.8, 35.8, 2.6));
            hublist.Add(new CHub("Shimano Deore XT FH-M737 Rear 135", 45.0, 45.0, 36.8, 23.2, 2.6));
            hublist.Add(new CHub("Shimano Deore XT FH-M756L Rear 135", 61.0, 61.0, 32.0, 18.5, 2.6));
            hublist.Add(new CHub("Shimano Deore XT HB-M737, HB-M738 Front 100", 38.0, 38.0, 35.8, 35.8, 2.6));
            hublist.Add(new CHub("Shimano Deore XT HB-M756L Front 100", 61.0, 61.0, 21.1, 31.7, 2.6));
            hublist.Add(new CHub("Shimano Dura-Ace 10spd 7800 Rear 130", 44.0, 44.0, 38.0, 22.2, 2.4));
            hublist.Add(new CHub("Shimano Dura-Ace 7800 Front 100", 40.0, 40.0, 38.7, 38.7, 2.4));
            hublist.Add(new CHub("Shimano Dura-Ace FH-7400 6S Rear 126", 44.0, 44.0, 36.8, 23.2, 2.4));
            hublist.Add(new CHub("Shimano Dura-Ace FH-7400 7S Rear 126", 44.0, 44.0, 38.2, 21.9, 2.4));
            hublist.Add(new CHub("Shimano Dura-Ace FH-7402, 7403 8S Rear 130", 44.0, 44.0, 36.9, 21.1, 2.4));
            hublist.Add(new CHub("Shimano Dura-Ace FH-7700 9S Rear 130", 44.0, 44.0, 36.9, 21.1, 2.4));
            hublist.Add(new CHub("Shimano Dura-Ace low flange, all Front 100", 38.0, 38.0, 37.0, 37.0, 2.4));
            hublist.Add(new CHub("Shimano Dura-Ace re-spaced to track Rear 120", 44.0, 44.0, 29.0, 29.0, 2.4));
            hublist.Add(new CHub("Shimano Dura-Ace track high flange Front 100", 67.0, 67.0, 34.0, 34.0, 2.6));
            hublist.Add(new CHub("Shimano Dura-Ace track high flange Rear 120", 67.0, 67.0, 41.0, 30.0, 2.6));
            hublist.Add(new CHub("Shimano Dura-Ace track small flange Front 100", 38.0, 38.0, 35.8, 35.8, 2.4));
            hublist.Add(new CHub("Shimano Dura-Ace track small flange Rear 120", 44.0, 44.0, 37.5, 31.7, 2.4));
            hublist.Add(new CHub("Shimano Exage FH-HG40, FH-HG50 Rear 126", 45.0, 45.0, 38.2, 20.2, 2.6));
            hublist.Add(new CHub("Shimano Exage FH-HG40, FH-HG50 Rear 130", 45.0, 45.0, 36.2, 22.2, 2.6));
            hublist.Add(new CHub("Shimano Exage FH-HG40, FH-HG50 Rear 135", 45.0, 45.0, 33.7, 24.7, 2.6));
            hublist.Add(new CHub("Shimano Exage HB-RM50-F, HB-RM50 Front 100", 38.0, 38.0, 36.3, 36.3, 2.6));
            hublist.Add(new CHub("Shimano FH-RM40-8 Rear 135", 45.0, 45.0, 35.6, 25.0, 2.6));
            hublist.Add(new CHub("Shimano HB-NX30 hubdynamo Front 100", 80.0, 80.0, 28.0, 28.0, 2.5));
            hublist.Add(new CHub("shimano hb-nx60-j (发电+罗拉刹型) Front 100", 80.0, 80.0, 23.0, 23.0, 2.5));
            hublist.Add(new CHub("Shimano Inter-L hub dynamo  Front 100", 80.0, 80.0, 29.0, 29.0, 2.8));
            hublist.Add(new CHub("Shimano Nexave Front 100", 52.0, 52.0, 25.0, 33.0, 2.8));
            hublist.Add(new CHub("Shimano Nexave Rear 135", 45.0, 45.0, 30.0, 21.0, 2.8));
            hublist.Add(new CHub("Shimano Nexus 4S coaster brake SG-4C30 Rear 123.5", 84.0, 84.0, 31.2, 23.5, 2.6));
            hublist.Add(new CHub("Shimano Nexus 4S roller brake SG-4R31 Rear 123.5", 84.0, 84.0, 27.7, 33.6, 2.6));
            hublist.Add(new CHub("Shimano Nexus 7S coaster brake SG-7C21 Rear 127", 87.0, 87.0, 29.7, 24.7, 2.6));
            hublist.Add(new CHub("Shimano Nexus 7S roller brake SG-7R40 Rear 130", 87.0, 87.0, 31.2, 23.5, 2.6));
            hublist.Add(new CHub("Shimano Nexus HB-IM40 front, roller brake Front 100", 52.0, 52.0, 26.0, 33.0, 2.6));
            hublist.Add(new CHub("Shimano Nexus HB-NX70 Disc hubdynamo Front 100", 80.0, 80.0, 23.2, 28.8, 2.5));
            hublist.Add(new CHub("Shimano Nexus Inter-8 8-speed internal Rear 132", 92.6, 92.6, 29.3, 24.9, 2.6));
            hublist.Add(new CHub("Shimano Nexus Inter-8 8-speed internal SG-8R20 Rear ", 92.6, 92.6, 30.3, 24.9, 2.6));
            hublist.Add(new CHub("Shimano Nexus Inter-8 8-speed internal without roller brake locknut Rear 125", 92.6, 92.6, 32.9, 21.3, 2.6));
            hublist.Add(new CHub("Shimano Nexus Inter-L HB-NX30 hub dynamo Front 100", 80.0, 80.0, 28.5, 28.5, 2.5));
            hublist.Add(new CHub("Shimano Nexus SG-7R45 7SPD INTER-7 ROLLER BRAKE Rear 130", 87.0, 87.0, 29.2, 25.4, 2.6));
            hublist.Add(new CHub("Shimano Nexus Sport DH-3D70 hub dynamo (disc brake) Front 100", 74.0, 74.0, 22.3, 30.0, 2.6));
            hublist.Add(new CHub("Shimano Nexus Sport DH-3N70 hub dynamo Front 100", 74.0, 74.0, 30.0, 30.0, 2.6));
            hublist.Add(new CHub("Shimano parallax  Front 100", 38.0, 38.0, 34.5, 34.5, 2.6));
            hublist.Add(new CHub("Shimano RSX FH-A410 Rear 126", 45.0, 45.0, 37.5, 20.7, 2.6));
            hublist.Add(new CHub("Shimano RSX FH-A410 Rear 130", 45.0, 45.0, 36.0, 22.2, 2.6));
            hublist.Add(new CHub("Shimano RSX FH-A416 Rear 130", 45.0, 45.0, 35.8, 18.8, 2.6));
            hublist.Add(new CHub("Shimano RSX FH-A416, respaced from 130mm to 135 Rear 135", 45.0, 45.0, 33.3, 21.3, 2.6));
            hublist.Add(new CHub("Shimano RSX HB-A410 Front 100", 38.0, 38.0, 36.3, 36.3, 2.6));
            hublist.Add(new CHub("Shimano RX100 FH-A550 Rear 126", 45.0, 45.0, 37.3, 20.7, 2.6));
            hublist.Add(new CHub("Shimano RX100 HB-A550-F Front 100", 38.0, 38.0, 36.3, 36.3, 2.6));
            hublist.Add(new CHub("Shimano Santé  Front 100", 45.0, 45.0, 35.5, 35.5, 2.6));
            hublist.Add(new CHub("Shimano Santé Rear 126", 45.0, 45.0, 36.8, 20.2, 2.6));
            hublist.Add(new CHub("shimano SG-8R31  (内八+罗拉刹型) Rear 115", 92.0, 92.0, 35.0, 35.0, 2.5));
            hublist.Add(new CHub("Shimano STX FH-MC32 Rear 130", 45.0, 45.0, 38.1, 22.5, 2.6));
            hublist.Add(new CHub("Shimano STX FH-MC32 Rear 135", 45.0, 45.0, 35.6, 25.0, 2.6));
            hublist.Add(new CHub("Shimano STX FH-R050 Rear 130", 45.0, 45.0, 38.1, 22.5, 2.6));
            hublist.Add(new CHub("Shimano STX FH-R050 Rear 135", 45.0, 45.0, 35.6, 25.0, 2.6));
            hublist.Add(new CHub("Shimano STX HB-MC32 Front 100", 38.0, 38.0, 35.8, 35.8, 2.6));
            hublist.Add(new CHub("Shimano STX-RC FH-MC33 Rear 130", 45.0, 45.0, 38.1, 22.5, 2.6));
            hublist.Add(new CHub("Shimano STX-RC FH-MC33 Rear 135", 45.0, 45.0, 35.6, 25.0, 2.6));
            hublist.Add(new CHub("Shimano STX-RC. HB-MC33-S, HB-MC33 Front 100", 38.0, 38.0, 35.8, 35.8, 2.6));
            hublist.Add(new CHub("Shimano tandem FH-FH08 Rear 145", 63.0, 63.0, 28.0, 22.0, 2.6));
            hublist.Add(new CHub("Shimano tandem HB-HF08 Front 100", 63.0, 63.0, 35.0, 35.0, 2.6));
            hublist.Add(new CHub("Shimano Ultegra 9 spd FH-6500 Rear 130", 44.0, 44.0, 38.0, 18.0, 2.4));
            hublist.Add(new CHub("Shimano Ultegra DH-2N70 hub dynamo Front 100", 74.0, 74.0, 28.5, 28.5, 2.5));
            hublist.Add(new CHub("Shimano Ultegra FH-6500 9S Rear 130", 45.0, 45.0, 37.1, 20.9, 2.6));
            hublist.Add(new CHub("Shimano Ultegra HB-6500 Front 100", 38.0, 38.0, 36.3, 36.3, 2.6));
            hublist.Add(new CHub("Shimano Ultegra HB-6500A Front 100", 38.0, 38.0, 38.7, 38.7, 2.4));
            hublist.Add(new CHub("Shimano XT Disc FH-M756 Rear 135", 45.0, 45.0, 37.0, 24.7, 2.6));
            hublist.Add(new CHub("Shimano XT Disc Front 100", 61.0, 61.0, 22.0, 33.0, 2.6));
            hublist.Add(new CHub("Shimano XT Disc HB-M765 Front 100", 44.0, 38.0, 26.5, 37.5, 2.6));
            hublist.Add(new CHub("Shimano XT Disc Rear 135", 61.0, 61.0, 34.0, 19.0, 2.6));
            hublist.Add(new CHub("Shimano XT FH-M750 Rear 135", 45.0, 45.0, 34.5, 21.0, 2.6));
            hublist.Add(new CHub("Shimano XT FH-M752 Rear 135", 44.0, 44.0, 37.0, 21.0, 2.6));
            hublist.Add(new CHub("Shimano XT FH-M760 Rear 135", 45.0, 45.0, 37.6, 21.8, 2.6));
            hublist.Add(new CHub("Shimano XT HB-M751 Front 100", 38.0, 38.0, 35.5, 35.5, 2.6));
            hublist.Add(new CHub("Shimano XT HB-M753 Front 100", 38.0, 38.0, 35.0, 35.0, 2.6));
            hublist.Add(new CHub("Shimano XT HB-M760 Front 100", 38.0, 38.0, 34.2, 34.2, 2.6));
            hublist.Add(new CHub("Shimano XT-II, DX Front 100", 38.0, 38.0, 35.8, 35.8, 2.6));
            hublist.Add(new CHub("Shimano XT-II, DX Rear 135", 45.0, 45.0, 35.6, 25.0, 2.6));
            hublist.Add(new CHub("Shimano XTR FH-M950 Rear 135", 45.0, 45.0, 36.8, 23.2, 2.6));
            hublist.Add(new CHub("Shimano XTR FH-M965 Disc Rear 135", 45.0, 45.0, 35.4, 23.1, 2.6));
            hublist.Add(new CHub("Shimano XTR HB-M950 Front 100", 38.0, 38.0, 35.8, 35.8, 2.6));
            hublist.Add(new CHub("Shimano XTR HB-M965 Disc Front 100", 43.4, 38.0, 24.8, 35.8, 2.6));
            hublist.Add(new CHub("Shogun Little Sumo Front 100", 40.0, 40.0, 30.0, 30.0, 2.8));
            hublist.Add(new CHub("Shogun Zero Carbon Front 100", 46.0, 46.0, 33.0, 33.0, 2.6));
            hublist.Add(new CHub("SON 20-28\" hubdynamo (2000 model)  Front 100", 70.0, 70.0, 30.0, 30.0, 2.5));
            hublist.Add(new CHub("SON 20-28\" hubdynamo-disc brake (2000 model) Front 100", 70.0, 70.0, 21.5, 30.0, 2.5));
            hublist.Add(new CHub("Specialized low flange  Front 100", 38.0, 38.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Specialized low flange  Rear 126", 44.5, 44.5, 35.5, 20.0, 2.4));
            hublist.Add(new CHub("Specialized low flange  Rear 130", 44.5, 44.5, 33.5, 22.0, 2.4));
            hublist.Add(new CHub("Specialized medium flange  Front 100", 43.0, 43.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Specialized medium flange  Rear old", 46.0, 46.0, 28.0, 22.0, 2.4));
            hublist.Add(new CHub("Specialized medium flange  Rear older", 46.0, 46.0, 33.0, 18.0, 2.4));
            hublist.Add(new CHub("Specialized skraxle by DT Front 100", 36.0, 36.0, 34.8, 34.8, 2.3));
            hublist.Add(new CHub("Specialized Stout Front 100", 45.0, 45.0, 37.0, 37.0, 2.6));
            hublist.Add(new CHub("Specialized Y2K by DT Front 100", 36.0, 36.0, 34.8, 34.8, 2.3));
            hublist.Add(new CHub("Specialized Y2K by DT Rear 130", 45.0, 45.0, 37.7, 16.7, 2.4));
            hublist.Add(new CHub("Specialized Y2K by DT Rear 135", 45.0, 45.0, 35.2, 19.2, 2.4));
            hublist.Add(new CHub("Speed Tec disc Front 100", 54.0, 54.0, 20.0, 32.0, 2.5));
            hublist.Add(new CHub("Speed Tec disc Rear 135", 54.0, 54.0, 34.0, 18.0, 2.5));
            hublist.Add(new CHub("Speed Tec Rear 130", 38.0, 54.0, 38.0, 18.0, 2.5));
            hublist.Add(new CHub("Spot Single Speed Front 100", 48.0, 48.0, 35.0, 35.0, 2.6));
            hublist.Add(new CHub("Spot single speed Rear 135", 48.0, 48.0, 42.0, 41.0, 2.6));
            hublist.Add(new CHub("SR sealed bearing Front 100", 40.0, 40.0, 34.3, 34.3, 2.6));
            hublist.Add(new CHub("SRAM 2001 non-disc hubs Front 100", 39.0, 39.0, 29.6, 29.6, 2.5));
            hublist.Add(new CHub("SRAM 2001 non-disc hubs Rear 135", 45.0, 45.0, 30.7, 20.5, 2.5));
            hublist.Add(new CHub("SRAM 5 speed hub MH 5205 w/no brake Rear 123", 75.0, 75.0, 29.0, 29.0, 3.0));
            hublist.Add(new CHub("SRAM 5 speed hub MH 5215 w/coaster brake Rear 122", 75.0, 75.0, 39.5, 28.5, 3.0));
            hublist.Add(new CHub("SRAM 5 speed hub MH 5225 w/drum brake Rear 126", 90.0, 90.0, 29.5, 30.5, 3.0));
            hublist.Add(new CHub("SRAM 9.0 Front 100", 39.0, 39.0, 30.0, 30.0, 2.6));
            hublist.Add(new CHub("SRAM 9.0SL disc Front 100", 45.0, 45.0, 23.0, 27.8, 2.5));
            hublist.Add(new CHub("SRAM 9.0SL, Disc Rear 135", 45.0, 45.0, 30.7, 20.5, 2.5));
            hublist.Add(new CHub("SRAM Dual Drive 27/24 disc brake Rear ", 67.0, 67.0, 33.0, 18.0, 2.6));
            hublist.Add(new CHub("SRAM Dual Drive 27/24 I-brake Rear ", 67.0, 67.0, 33.0, 18.0, 2.6));
            hublist.Add(new CHub("SRAM Dual Drive 27/24 Rear ", 67.0, 67.0, 33.0, 18.0, 2.6));
            hublist.Add(new CHub("SRAM i-Motion 9 Gearhub (with back pedal brake) Rear 135", 93.6, 93.6, 26.5, 31.5, 2.6));
            hublist.Add(new CHub("SRAM Neos, Centera Rear 135", 45.0, 45.0, 27.4, 23.8, 2.5));
            hublist.Add(new CHub("SRAM Neos, Centera, Traax Rear 130", 45.0, 45.0, 29.9, 21.3, 2.5));
            hublist.Add(new CHub("SRAM Plasma, Quartz, Neos, Centera Rear 135", 45.0, 45.0, 30.4, 20.8, 2.5));
            hublist.Add(new CHub("SRAM Plasma, Quartz, Neos, Centera, Traax Front 100", 39.0, 39.0, 29.6, 29.6, 2.5));
            hublist.Add(new CHub("SRAM Power Disc: Plasma, Quartz Rear 135", 62.5, 62.5, 30.4, 20.8, 2.5));
            hublist.Add(new CHub("SRAM Power Disc: Quartz Rear 130", 62.5, 62.5, 32.9, 18.3, 2.5));
            hublist.Add(new CHub("SRAM Quartz, Neos, Centera Rear 130", 45.0, 45.0, 32.9, 18.3, 2.5));
            hublist.Add(new CHub("SRAM Sparc Rear ", 194.0, 194.0, 27.5, 27.5, 2.9));
            hublist.Add(new CHub("SRAM Spectro 12 speed hub with coaster brake Rear 135", 105.0, 132.0, 37.9, 20.7, 2.6));
            hublist.Add(new CHub("SRAM Spectro 3 speed hum MH 3115 w/no brake Rear 117", 58.0, 58.0, 25.5, 24.5, 3.0));
            hublist.Add(new CHub("SRAM Spectro 3 speed hum MH 3125 w/coaster brake Rear 118", 58.0, 58.0, 25.5, 24.5, 3.0));
            hublist.Add(new CHub("SRAM Spectro 3 speed hum MH 3125 w/drum brake Rear 118", 89.0, 89.0, 32.5, 25.5, 3.0));
            hublist.Add(new CHub("SRAM Spectro 3x7: 3s hub with 7s cassette Rear 135", 67.0, 67.0, 35.0, 20.0, 2.6));
            hublist.Add(new CHub("SRAM Spectro 7 speed hub MH 7205 w/no brake Rear 132", 75.0, 75.0, 34.5, 34.0, 3.0));
            hublist.Add(new CHub("SRAM Spectro 7 speed hub MH 7215 w/coaster brake Rear 130", 75.0, 75.0, 34.0, 33.0, 3.0));
            hublist.Add(new CHub("SRAM Spectro 7 speed hub MH 7225 w/drum brake Rear 135", 90.0, 90.0, 35.7, 34.8, 3.0));
            hublist.Add(new CHub("SRAM Traax Rear 130", 45.0, 45.0, 30.8, 20.4, 2.5));
            hublist.Add(new CHub("starhubs 20孔培林公路前花鼓 Front 100", 38.0, 38.0, 33.0, 33.0, 3.0));
            hublist.Add(new CHub("STARHUBS 28孔前花鼓 Front 74", 46.0, 46.0, 27.0, 27.0, 2.5));
            hublist.Add(new CHub("STARHUBS 2培林公路后花鼓 Rear 130", 45.0, 45.0, 35.0, 18.0, 2.8));
            hublist.Add(new CHub("starhubs28孔碟刹前花鼓 Front 100", 57.0, 57.0, 20.0, 36.0, 3.5));
            hublist.Add(new CHub("Starhubs公路規格20孔轴承4培林棘轮鋁花鼓（此款已停产） Rear 130", 43.0, 43.0, 35.0, 19.0, 3.0));
            hublist.Add(new CHub("Sturmey-Archer 3 spd  Rear ", 65.0, 65.0, 27.0, 27.0, 2.4));
            hublist.Add(new CHub("Sturmey-Archer dyno  Front 100", 102.5, 42.5, 27.0, 27.0, 2.4));
            hublist.Add(new CHub("Sturmey-Archer dyno  Rear ", 101.0, 65.0, 40.0, 25.5, 2.4));
            hublist.Add(new CHub("Sturmey-Archer Elite ST drum  Rear ", 90.0, 90.0, 41.5, 18.5, 2.4));
            hublist.Add(new CHub("Sturmey-Archer hi brake  Front 100", 90.0, 90.0, 27.0, 27.0, 2.4));
            hublist.Add(new CHub("Sturmey-Archer hi brake  Rear ", 82.0, 82.0, 40.0, 20.0, 2.4));
            hublist.Add(new CHub("Sunn by DT Swiss Rear 135", 45.0, 45.0, 37.2, 19.7, 2.4));
            hublist.Add(new CHub("Sun-Ringlé 439/439 Lite Rear 135", 52.0, 52.0, 34.0, 21.0, 2.4));
            hublist.Add(new CHub("Sun-Ringlé ABBAH / Lawwill DH (12mm thru axle) Rear 135", 54.5, 54.5, 35.5, 20.5, 2.4));
            hublist.Add(new CHub("Sun-Ringlé ABBAH S.O.S. DH (20mm thru axle) Front 110", 59.5, 54.5, 24.5, 35.5, 2.6));
            hublist.Add(new CHub("Sun-Ringlé ABBAH S.O.S. XC (9mm axle ends) Front 100", 59.5, 45.5, 21.0, 35.0, 2.6));
            hublist.Add(new CHub("Sun-Ringlé ABBAH S.O.S. XC Rear 135", 59.5, 59.5, 34.5, 21.0, 2.6));
            hublist.Add(new CHub("Sun-Ringlé Atilla the Hub Front 95?", 51.0, 51.0, 32.1, 32.1, 2.6));
            hublist.Add(new CHub("Sun-Ringlé Attila the hub (BMX) Rear 110", 51.0, 51.0, 37.1, 30.2, 2.6));
            hublist.Add(new CHub("Sun-Ringlé SuperDuper Bubba  Front 100", 46.0, 46.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("Suntour 7 spd 135mm cass  Rear 135", 44.5, 44.5, 32.0, 22.0, 2.4));
            hublist.Add(new CHub("Suntour GPX Front 100", 39.0, 39.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Suntour high flange track  Front 100", 67.0, 67.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Suntour high flange track  Rear 120", 67.0, 67.0, 36.0, 30.0, 2.4));
            hublist.Add(new CHub("Suntour low flange  Rear 126", 44.5, 44.5, 35.5, 20.0, 2.4));
            hublist.Add(new CHub("Suntour low flange  Rear 135", 44.5, 44.5, 31.5, 24.0, 2.4));
            hublist.Add(new CHub("Suntour micro drive 7 spd  Rear 135", 44.5, 44.5, 31.0, 20.5, 2.4));
            hublist.Add(new CHub("Suntour micro drive 8 spd  Rear 135", 44.5, 44.5, 33.5, 21.5, 2.4));
            hublist.Add(new CHub("Suntour Microlight Front 100", 36.0, 36.0, 34.0, 34.0, 2.5));
            hublist.Add(new CHub("Suntour Sprint Front 100", 39.0, 39.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Suntour Sprint low flange Rear 126", 44.0, 44.0, 36.0, 19.0, 2.4));
            hublist.Add(new CHub("Suntour Superbe low flange Rear 126", 44.5, 44.5, 36.0, 19.0, 2.4));
            hublist.Add(new CHub("Suntour Superbe Pro cassette Rear 130", 44.5, 44.5, 37.0, 17.0, 2.4));
            hublist.Add(new CHub("Suntour Superbe Pro Front 100", 39.0, 39.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Suntour XC-9000 Front 100", 39.0, 39.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Suntour XC-9000, XCD6000, XCD9000 Rear 130", 45.0, 45.0, 34.0, 21.0, 2.4));
            hublist.Add(new CHub("Suntour XCD-6000 Front 100", 36.0, 36.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Surly \"New\" disc Front 100", 58.0, 58.0, 22.5, 32.0, 2.4));
            hublist.Add(new CHub("Surly \"New\" disc Rear 135", 58.0, 58.0, 34.0, 38.5, 2.4));
            hublist.Add(new CHub("Surly \"New\" single speed MTB hub Rear 135", 54.0, 54.0, 38.4, 38.4, 2.4));
            hublist.Add(new CHub("Surly \"New\" track hub Front 100", 54.0, 54.0, 32.2, 32.2, 2.4));
            hublist.Add(new CHub("Surly \"New\" track hub Rear 120", 54.0, 54.0, 30.1, 30.1, 2.4));
            hublist.Add(new CHub("Surly 1x1 Front 100", 43.0, 43.0, 33.0, 33.0, 2.4));
            hublist.Add(new CHub("Surly 1x1 single speed Rear 135", 43.0, 43.0, 39.0, 39.0, 2.4));
            hublist.Add(new CHub("Suzue front high flange Front 100", 62.0, 62.0, 31.0, 31.0, 2.6));
            hublist.Add(new CHub("Suzue Sil-SP Front 100", 62.5, 62.5, 31.3, 31.3, 2.4));
            hublist.Add(new CHub("Suzue Sil-SP Rear 120", 62.5, 62.5, 27.3, 27.3, 2.4));
            hublist.Add(new CHub("Suzue Tandem Rear ", 63.0, 63.0, 27.0, 27.0, 2.5));
            hublist.Add(new CHub("Suzue unicycle hub n/a n/a", 63.9, 63.9, 30.6, 30.6, 2.6));
            hublist.Add(new CHub("Syncros disc brake  Front 100", 59.8, 39.8, 22.1, 34.3, 2.4));
            hublist.Add(new CHub("Syncros Hardcore Front 100", 39.8, 39.8, 34.3, 34.3, 2.4));
            hublist.Add(new CHub("TNT 8-speed  Rear 130", 32.0, 45.0, 35.0, 18.0, 2.4));
            hublist.Add(new CHub("TNT 8-speed  Rear 135", 32.0, 45.0, 35.0, 22.0, 2.4));
            hublist.Add(new CHub("TNT standard Front 100", 32.0, 32.0, 37.5, 37.5, 2.4));
            hublist.Add(new CHub("TNT suspension 8-speed  Rear 135", 45.0, 45.0, 33.0, 19.0, 2.4));
            hublist.Add(new CHub("TNT suspension Front 100", 42.0, 42.0, 39.0, 39.0, 2.4));
            hublist.Add(new CHub("Tomo Front 100", 41.0, 41.0, 35.0, 35.0, 2.6));
            hublist.Add(new CHub("TruVative Disc Sealex SL FH-DISCX-SL-00 Front 100", 61.0, 45.0, 23.6, 31.9, 2.4));
            hublist.Add(new CHub("TruVative Sealex SL Disc RH-DISCX-SL-00 Rear 135", 61.0, 61.0, 34.1, 19.2, 2.4));
            hublist.Add(new CHub("TruVative Sealex SL FH-SEAL-SL-00 Front 100", 38.0, 38.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("TruVative Sealex SL RH-SEAL-SL-00 Rear 135", 45.0, 61.0, 32.0, 19.5, 2.4));
            hublist.Add(new CHub("TruVative Sealex XR FH-SEAL-XR-00 Front 100", 38.0, 38.0, 34.0, 34.0, 2.4));
            hublist.Add(new CHub("TruVative Sealex XR RH-SEAL-XR-00 Rear 135", 45.0, 61.0, 32.0, 19.5, 2.4));
            hublist.Add(new CHub("Tune King disc 2000 Front 100", 56.0, 56.0, 22.0, 35.0, 2.5));
            hublist.Add(new CHub("Tune King disc 99 DH Front 110", 56.0, 56.0, 19.0, 34.0, 2.5));
            hublist.Add(new CHub("Tune King Gustav M 99 Front 100", 59.0, 59.0, 20.0, 31.0, 2.4));
            hublist.Add(new CHub("Tune King Gustav M 99 Rear 135", 59.0, 59.0, 31.0, 20.0, 2.4));
            hublist.Add(new CHub("Tune King Louise 99 Front 100", 56.0, 56.0, 24.0, 35.0, 2.5));
            hublist.Add(new CHub("Tune King MK disc 99 DH Front 110", 55.5, 55.5, 25.0, 34.0, 2.5));
            hublist.Add(new CHub("Tune Kong disc Rear 135", 56.0, 56.0, 36.0, 20.0, 2.5));
            hublist.Add(new CHub("Tune Kong Louise 99 Rear 135", 56.0, 56.0, 31.0, 20.0, 2.5));
            hublist.Add(new CHub("Tune MAG180/200/215  183,205,225gr Rear 130", 41.0, 54.0, 39.0, 17.0, 2.4));
            hublist.Add(new CHub("Tune MAG180/200/215  183,205,225gr Rear 135", 41.0, 54.0, 37.0, 20.0, 2.4));
            hublist.Add(new CHub("Tune MIG 66 74gr Front 100", 35.6, 35.6, 34.0, 34.0, 2.3));
            hublist.Add(new CHub("Tune MIG 75 Front 100", 36.0, 36.0, 36.0, 36.0, 2.5));
            hublist.Add(new CHub("Tune Power Tap Prologue Rear 130", 78.0, 66.0, 36.2, 16.7, 2.4));
            hublist.Add(new CHub("Tune SandWitch Front 100", 51.6, 51.6, 42.0, 42.0, 2.5));
            hublist.Add(new CHub("Ultimate Machine Co. road Front 100", 32.0, 32.0, 34.0, 34.0, 2.3));
            hublist.Add(new CHub("Ultimate Machine Co., small left flange Rear 130", 32.0, 43.5, 34.0, 21.0, 2.3));
            hublist.Add(new CHub("Ultimate Machine Co., two equal flange diameters Rear 130", 43.5, 43.5, 34.0, 21.0, 2.3));
            hublist.Add(new CHub("Union/Marwi WingIII Front ", 81.0, 81.0, 28.0, 28.0, 2.6));
            hublist.Add(new CHub("Wheelring wheelchair Rear ", 44.0, 44.0, 32.0, 32.0, 2.4));
            hublist.Add(new CHub("Wheelsmith Bullet  Front 100", 38.0, 38.0, 40.0, 40.0, 2.4));
            hublist.Add(new CHub("Wheelsmith Bullet cass.  Rear 130", 44.0, 44.0, 35.0, 20.0, 2.4));
            hublist.Add(new CHub("White Industries cassette Rear 130", 54.0, 54.0, 34.3, 20.1, 2.4));
            hublist.Add(new CHub("White Industries cassette Rear 135", 54.0, 54.0, 34.5, 21.5, 2.4));
            hublist.Add(new CHub("White Industries disc Front 100", 74.0, 74.0, 21.0, 32.0, 2.6));
            hublist.Add(new CHub("White Industries ENO eccentric flip-flop Rear 126, 130, 135", 48.0, 48.0, 32.0, 32.0, 2.5));
            hublist.Add(new CHub("White Industries Hayes disc DH Front 110", 75.0, 75.0, 22.0, 35.0, 2.6));
            hublist.Add(new CHub("White Industries Hayes disc Rear 135", 67.0, 67.0, 36.0, 21.0, 2.6));
            hublist.Add(new CHub("White Industries LTA cassette Rear 130", 37.0, 54.5, 36.0, 18.7, 2.5));
            hublist.Add(new CHub("White Industries road Front 100", 34.0, 34.0, 38.0, 38.0, 2.3));
            hublist.Add(new CHub("White Industries Ti  Front 100", 34.0, 34.0, 35.5, 35.5, 2.4));
            hublist.Add(new CHub("White Industries Ti  Rear 126", 54.0, 54.0, 29.5, 24.5, 2.4));
            hublist.Add(new CHub("White Industries Ti  Rear 126", 54.0, 54.0, 37.0, 19.0, 2.4));
            hublist.Add(new CHub("White Industries Ti  Rear 130", 54.0, 54.0, 27.0, 27.0, 2.4));
            hublist.Add(new CHub("White Industries Ti  Rear 130", 54.0, 54.0, 32.0, 22.0, 2.4));
            hublist.Add(new CHub("White Industries Ti  Rear 130", 54.0, 54.0, 36.0, 18.0, 2.4));
            hublist.Add(new CHub("White Industries Ti  Rear 135", 54.0, 54.0, 29.0, 25.0, 2.4));
            hublist.Add(new CHub("White Industries Ti  Rear 135", 54.0, 54.0, 34.0, 20.0, 2.4));
            hublist.Add(new CHub("White Industries Tracker  Front 100", 40.0, 40.0, 35.5, 35.5, 2.4));
            hublist.Add(new CHub("Wilderness Trails classic narrow  Front 100", 47.0, 47.0, 36.0, 36.0, 2.4));
            hublist.Add(new CHub("Wilderness Trails classic standard  Front 100", 47.0, 47.0, 39.0, 39.0, 2.4));
            hublist.Add(new CHub("Wilderness Trails classic wide  Front 100", 47.0, 47.0, 45.0, 45.0, 2.4));
            hublist.Add(new CHub("Wilderness Trails Momentum  Front 100", 40.0, 40.0, 34.5, 34.5, 2.4));
            hublist.Add(new CHub("Wilderness Trails New Paradigm large flange Front 100", 45.0, 45.0, 39.0, 39.0, 2.4));
            hublist.Add(new CHub("Wilderness Trails New Paradigm small flange Front 100", 42.5, 42.5, 39.0, 39.0, 2.4));
            hublist.Add(new CHub("Wilderness Trails Rear 136", 59.0, 59.0, 27.0, 27.0, 2.4));
            hublist.Add(new CHub("Wilderness Trails Rear 141", 59.0, 59.0, 24.5, 29.5, 2.4));
            hublist.Add(new CHub("Winners lo flange  Front 100", 36.0, 36.0, 35.0, 35.0, 2.4));
            hublist.Add(new CHub("Winners low flange  Rear 135", 41.5, 41.5, 36.0, 20.5, 2.4));
            hublist.Add(new CHub("Woodman Bill LFY for Cannondale Lefty fork Front ", 58.0, 58.0, 16.0, 35.0, 2.5));
            hublist.Add(new CHub("WTB classic WTB 7  Rear 131", 59.0, 59.0, 30.0, 25.0, 2.4));
            hublist.Add(new CHub("WTB classic WTB 7  Rear 136", 59.0, 59.0, 28.0, 27.0, 2.4));
            hublist.Add(new CHub("WTB classic WTB 8  Rear 136", 59.0, 59.0, 31.5, 23.5, 2.4));
            hublist.Add(new CHub("WTB classic WTB 8  Rear 141", 59.0, 59.0, 27.5, 27.5, 2.4));
            hublist.Add(new CHub("WTB New Paradigm 7-speed cassette  Rear 135", 45.0, 45.0, 31.0, 25.0, 2.4));
            hublist.Add(new CHub("WTB New Paradigm 7-speed cassette  Rear 140", 45.0, 45.0, 38.5, 27.5, 2.4));
            hublist.Add(new CHub("WTB New Paradigm 8-speed cassette  Rear 136", 45.0, 45.0, 33.0, 23.0, 2.4));
            hublist.Add(new CHub("WTB New Paradigm 8-speed cassette  Rear 139", 45.0, 45.0, 35.0, 21.0, 2.4));
            hublist.Add(new CHub("Xero Lite 16H Rear 130", 44.0, 37.0, 35.3, 19.1, 2.6));
            hublist.Add(new CHub("XS BMX Front 100", 38.0, 38.0, 35.0, 35.0, 2.8));
            hublist.Add(new CHub("XS BMX high flange Front 100", 63.0, 63.0, 33.0, 33.0, 2.8));
            hublist.Add(new CHub("XS BMX high flange Rear 110", 63.0, 63.0, 29.0, 29.0, 2.8));
            hublist.Add(new CHub("XS BMX Rear 110", 45.0, 45.0, 27.0, 27.0, 2.8));
            hublist.Add(new CHub("Zipp  Rear 130", 43.0, 43.0, 35.0, 20.0, 2.4));
            hublist.Add(new CHub("Zipp 100 Front 100", 43.0, 43.0, 36.0, 36.0, 2.3));
            hublist.Add(new CHub("Zipp 217 Rear 130", 43.0, 43.0, 37.0, 20.0, 2.3));
            hublist.Add(new CHub("昆腾28孔滚珠花鼓 Rear 130", 45.0, 45.0, 38.0, 15.0, 3.0));
            hublist.Add(new CHub("清豪ROTAZ SHIMANO塔基 Rear 130", 60.0, 60.0, 27.4, 16.1, 2.6));
            hublist.Add(new CHub("政科28孔2培林前花鼓ZK918QB Front 74", 38.0, 38.0, 24.0, 24.0, 2.5));











            rimlist.Add(new CRim(Properties.Resources.StringOther, 600, 0));
            rimlist.Add(new CRim("11款sp8 WTB freedom轮圈 20X1.5 406", 383.0, 0.0));
            rimlist.Add(new CRim("Accel  24\" 管胎专用", 481.0, 0.0));
            rimlist.Add(new CRim("Accel  650C 管胎专用", 532.0, 0.0));
            rimlist.Add(new CRim("ACS Z  20\" BMX 406", 392.0, 0.0));
            rimlist.Add(new CRim("Alesa 913 - MEASURE TO BE SURE! 700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("Alesa Alloy 219 Brompton standard rim 16x1 3/8 349", 345.0, 0.0));
            rimlist.Add(new CRim("Alesa Explorer 20\" BMX 406", 378.0, 0.0));
            rimlist.Add(new CRim("ALEX DA16 451 20x1 1/8 451", 428.0, 0.0));
            rimlist.Add(new CRim("Alex DA22 700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("Alex DM24 16x1.75 305", 288.0, 0.0));
            rimlist.Add(new CRim("Alex DV15 700C 622", 606.0, 0.0));
            rimlist.Add(new CRim("Alex DX32 26\" MTB 559", 553.0, 0.0));
            rimlist.Add(new CRim("Alex G6000 22mm wide x 29mm deep, 686g 700C 622", 583.8, 0.0));
            rimlist.Add(new CRim("Alex R390 (19.6mm wide, 19.1mm tall, double eyelets, 480g) 700C 622", 601.0, 0.0));
            rimlist.Add(new CRim("Alex R390 19.6w x 19.1h, double eyelets (MEASURE TO BE SURE!) 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("alex雅力士 DP20碟刹36孔 26\" MTB 559", 538.9, 0.0));
            rimlist.Add(new CRim("Ambrosio aero (with washers) 700C 管胎专用", 604.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Aero Elite (rim washers) 27\" 630", 611.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Aero Elite 700C 622", 605.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Area 4 (Touring/Trekking) 700C 622", 601.0, 0.0));
            rimlist.Add(new CRim("Ambrosio B.8 (Touring/Trekking) 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Balance 20w x 22h - MEASURE ERD TO BE SURE! 650C 622", 543.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Balance 20w x 22h - MEASURE ERD TO BE SURE! 700C 622", 594.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Balance 20w x 22h - MEASURE ERD TO BE SURE! 700C 622", 601.0, 0.0));
            rimlist.Add(new CRim("Ambrosio C.C.28 (Touring/Trekking) 700C 622", 602.5, 0.0));
            rimlist.Add(new CRim("Ambrosio Camel (Touring/Trekking) 700C 622", 588.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Crono 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Crono F.20 - MEASURE TO BE SURE 650C 管胎专用", 559.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Crono F.20 - MEASURE TO BE SURE 700C 管胎专用", 610.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Elite (single eyelets) 27\" 630", 620.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Elite 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Elite City 22 700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Elite Prisma 700C 622", 608.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Eurosport (Touring/Trekking) 700C 622", 606.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Evolution 700C 622", 606.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Excellence - MEASURE TO BE SURE 700C 622", 601.5, 0.0));
            rimlist.Add(new CRim("Ambrosio Excellence - MEASURE TO BE SURE 700C 622", 606.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Excellence 700C 管胎专用", 601.5, 0.0));
            rimlist.Add(new CRim("Ambrosio Excellight S.S.C. 700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Excursion 700C 622", 600.5, 0.0));
            rimlist.Add(new CRim("Ambrosio FCS 28 700C 622", 590.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Formula 20 Crono - MEASURE TO BE SURE 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Keba (Touring/Trekking) - MEASURE TO BE SURE! 700C 622", 598.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Metamorphosis 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Montreal - MEASURE TO BE SURE! 650C 管胎专用", 560.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Montreal - MEASURE TO BE SURE! 700C 管胎专用", 610.5, 0.0));
            rimlist.Add(new CRim("Ambrosio Montreal - MEASURE TO BE SURE! 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Nemesis 2000 700C 管胎专用", 610.5, 0.0));
            rimlist.Add(new CRim("Ambrosio Nemesis 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Nexus 700C 622", 608.4, 0.0));
            rimlist.Add(new CRim("Ambrosio Super Elite 700C 622", 615.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Synthesis 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Ambrosio The Frog (Touring/Trekking) 700C 622", 601.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Thesis 700C 管胎专用", 594.0, 0.0));
            rimlist.Add(new CRim("Ambrosio Tour de France 700C 管胎专用", 605.0, 0.0));
            rimlist.Add(new CRim("American Classic carbon fiber V rim 650C 571", 482.0, 0.0));
            rimlist.Add(new CRim("American Classic carbon fiber V rim 650C 管胎专用", 520.0, 0.0));
            rimlist.Add(new CRim("American Classic carbon fiber V rim 700C 管胎专用", 567.0, 0.0));
            rimlist.Add(new CRim("American Classic carbon fiber V rim by Zipp 700C 622", 528.0, 0.0));
            rimlist.Add(new CRim("American Classic CR-350, new 24mm deep model 700C 622", 590.5, 0.0));
            rimlist.Add(new CRim("American Classic Design's \"CR350\" 700C 622", 598.0, 0.0));
            rimlist.Add(new CRim("Answer Aero Tech 20\" BMX 406", 376.9, 0.0));
            rimlist.Add(new CRim("Araya 15 700C 622", 615.0, 0.0));
            rimlist.Add(new CRim("Araya 16A(3) 27\" 630", 623.0, 0.0));
            rimlist.Add(new CRim("Araya 16A(3) 700C 622", 616.0, 0.0));
            rimlist.Add(new CRim("Araya 16A(5) 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Araya 16A(5) 700C 622", 615.0, 0.0));
            rimlist.Add(new CRim("Araya 16B Gold 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Araya 16B Red 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Araya 16BGOLDNEW track, classic tub, 335g 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Araya 18 700C 622", 616.0, 0.0));
            rimlist.Add(new CRim("Araya 20A  (MEASURE TO BE SURE!) 24x1 1/8 520", 508.1, 0.0));
            rimlist.Add(new CRim("Araya 20A  20x1 1/8 451", 438.7, 0.0));
            rimlist.Add(new CRim("Araya 20A 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Araya 20A 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Araya 7X  24\" BMX 507", 497.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-1 700C 管胎专用", 609.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-1W  27\" 630", 617.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-1W 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-2 700C 管胎专用", 607.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-2W 27\" 630", 610.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-2W 700C 622", 602.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-4 24\" 管胎专用", 506.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-4 650C 管胎专用", 558.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-4 700C 管胎专用", 611.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-5 700C 管胎专用", 611.0, 0.0));
            rimlist.Add(new CRim("Araya ADX-510 (MEASURE TO BE SURE!) 650C 571", 549.3, 0.0));
            rimlist.Add(new CRim("Araya ADX-510 700C 622", 600.3, 0.0));
            rimlist.Add(new CRim("Araya Big Willie MG2000 (MEASURE TO BE SURE!) 26\" MTB 559", 538.6, 0.0));
            rimlist.Add(new CRim("Araya CT19 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Araya CTL185 700C 622", 609.0, 0.0));
            rimlist.Add(new CRim("Araya CTL-370 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Araya CTL385  700C 622", 609.0, 0.0));
            rimlist.Add(new CRim("Araya GP-710 (MEASURE TO BE SURE!) 16x1.75 305", 262.8, 0.0));
            rimlist.Add(new CRim("Araya GP-710 (MEASURE TO BE SURE!) 20\" BMX 406", 393.0, 0.0));
            rimlist.Add(new CRim("Araya GP-710 (MEASURE TO BE SURE!) 26\" MTB 559", 549.3, 0.0));
            rimlist.Add(new CRim("Araya LP-60 27\" 630", 616.4, 0.0));
            rimlist.Add(new CRim("Araya MP-707X (MEASURE TO BE SURE!) 20\" BMX 406", 396.4, 0.0));
            rimlist.Add(new CRim("Araya NEWADX-1S 20\" 管胎专用", 409.0, 0.0));
            rimlist.Add(new CRim("Araya NEWADX-1S 24\" 管胎专用", 503.2, 0.0));
            rimlist.Add(new CRim("Araya NEWADX-1S 650C 管胎专用", 553.3, 0.0));
            rimlist.Add(new CRim("Araya NEWADX-1S 700C 管胎专用", 606.3, 0.0));
            rimlist.Add(new CRim("Araya Pro Staff 340 700C 管胎专用", 619.0, 0.0));
            rimlist.Add(new CRim("Araya Pro Staff 400 700C 管胎专用", 619.0, 0.0));
            rimlist.Add(new CRim("Araya PX35 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Araya PX-645 (MEASURE TO BE SURE!) 700C 622", 610.4, 0.0));
            rimlist.Add(new CRim("Araya R-50 650C 管胎专用", 563.4, 0.0));
            rimlist.Add(new CRim("Araya R-50, classic tub, 415g 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Araya RB-17 (MEASURE TO BE SURE!) 20\" BMX 406", 379.0, 0.0));
            rimlist.Add(new CRim("Araya RB-17 (MEASURE TO BE SURE!) 24\" BMX 507", 498.7, 0.0));
            rimlist.Add(new CRim("Araya RB-17 (MEASURE TO BE SURE!) 26\" MTB 559", 498.7, 0.0));
            rimlist.Add(new CRim("Araya RB-907X/Super7X (MEASURE TO BE SURE!) 20\" BMX 406", 377.9, 0.0));
            rimlist.Add(new CRim("Araya RB-907X/Super7X (MEASURE TO BE SURE!) 24\" BMX 507", 499.0, 0.0));
            rimlist.Add(new CRim("Araya RB-J1/HIMIKO (MEASURE TO BE SURE!) 20\" BMX 406", 382.0, 0.0));
            rimlist.Add(new CRim("Araya RC-540 650C 571", 547.2, 0.0));
            rimlist.Add(new CRim("Araya RC-540 700C 622", 606.3, 0.0));
            rimlist.Add(new CRim("Araya RM14 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Araya RM17 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Araya RM20  24\" BMX 507", 496.0, 0.0));
            rimlist.Add(new CRim("Araya RM20 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Araya RM25 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Araya RM395 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Araya RM400 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Araya RM-915DH (MEASURE TO BE SURE!) 26\" MTB 559", 547.2, 0.0));
            rimlist.Add(new CRim("Araya RM-930DISC (MEASURE TO BE SURE!) 26\" MTB 559", 539.6, 0.0));
            rimlist.Add(new CRim("Araya RT-520 (MEASURE TO BE SURE!) 700C 622", 612.4, 0.0));
            rimlist.Add(new CRim("Araya SA-230S Super Aero (30mm deep) 24\" 管胎专用", 478.2, 0.0));
            rimlist.Add(new CRim("Araya SA-230S Super Aero (30mm deep) 650C 管胎专用", 530.0, 0.0));
            rimlist.Add(new CRim("Araya SA-230S Super Aero (30mm deep) 700C 管胎专用", 581.4, 0.0));
            rimlist.Add(new CRim("Araya SA-530C Super Aero clincher (30mm deep) 650C 571", 529.0, 0.0));
            rimlist.Add(new CRim("Araya SA-530C Super Aero clincher (30mm deep) 700C 622", 580.0, 0.0));
            rimlist.Add(new CRim("Araya SP-20 700C 622", 615.0, 0.0));
            rimlist.Add(new CRim("Araya SP-30 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Araya SP-30 700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Araya SS-40 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Araya SS-40 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Araya SS-45 27\" 630", 621.0, 0.0));
            rimlist.Add(new CRim("Araya SS-45 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Araya TB-807X/Ultra7X (MEASURE TO BE SURE!) 20\" BMX 406", 392.3, 0.0));
            rimlist.Add(new CRim("Araya TF-110 27\" 630", 616.4, 0.0));
            rimlist.Add(new CRim("Araya Tita-Ace 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Araya TM-840F (MEASURE TO BE SURE!) 26\" MTB 559", 549.3, 0.0));
            rimlist.Add(new CRim("Araya TX-310F (MEASURE TO BE SURE!) 700C 622", 610.4, 0.0));
            rimlist.Add(new CRim("Araya TX350 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Araya VP-20 (MEASURE TO BE SURE!) 24\" BMX 507", 498.7, 0.0));
            rimlist.Add(new CRim("Araya VP20 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Araya VX300  700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Araya VX400 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Araya W/O-2 steel 27\" 630", 618.4, 0.0));
            rimlist.Add(new CRim("Araya XA-1 26\" MTB 559", 543.0, 0.0));
            rimlist.Add(new CRim("Atomic Trailpimp 26\" MTB 559", 522.0, 0.0));
            rimlist.Add(new CRim("Avenir Duro 17  26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("AVIATE双星小刀圈 （贴纸写着Breeze） 16x1.5 305", 287.0, 0.0));
            rimlist.Add(new CRim("Bontrager BCX Blue  26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Bontrager BCX Red 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Bontrager BCX1, BCX2, BCX3  26\" MTB 559", 553.0, 0.0));
            rimlist.Add(new CRim("Bontrager carbon aero, back-calculated from 221441 using 255, 257 spokes. 650C 571", 534.0, 0.0));
            rimlist.Add(new CRim("Bontrager Mack 20\" BMX 406", 390.0, 0.0));
            rimlist.Add(new CRim("Bontrager Mack 24\" BMX 507", 495.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 201752 Mustang OSB Disc 26\" MTB 559", 539.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 211931 Mustang tubeless 26\" MTB 559", 542.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 220042 Race X-Lite alu 700C 管胎专用", 595.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 220421 Race X-Lite road  700C 622", 595.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 220429 Race Lite road OSB 700C 622", 598.0, 1.5));
            rimlist.Add(new CRim("Bontrager PN 220430 Race Lite road 650C 571", 546.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 220437 Select OSB 700C 622", 604.1, 1.5));
            rimlist.Add(new CRim("Bontrager PN 220459 Race Mod Tubeless front 26\" MTB 559", 542.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 220472 Race Tubeless 29\" OSB rim brake compatible  700C 622", 607.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 220481 Race Mod Disc specific 26\" MTB 559", 539.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 221065 Race X-Lite OCLV Carbon Aero 700C 管胎专用", 587.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 221636 Race X-Lite alu OSB 700C 管胎专用", 598.0, 1.5));
            rimlist.Add(new CRim("Bontrager PN 221715 Race Lite Tandem 700C 622", 580.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 221850 Race Lite Tubeless 29\" OSB rim brake compatible 700C 622", 607.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 230855 Race Lite Disc Tubeless OSB 26\" 559", 540.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 231248 Race X-Lite OSB 26\" MTB 559", 543.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 231265 Race Lite OSB road 650C 571", 547.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 231404 Race X-Light Aero aluminum clincher 700C 622", 598.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 231573 Race X-Lite alu, new wider rim 700C 管胎专用", 595.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 231574 Race X-Lite alu OSB, new wider rim 700C 管胎专用", 598.0, 1.5));
            rimlist.Add(new CRim("Bontrager PN 231714 Race X-Lite symmetrical 26\" MTB 559", 538.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 240064 RXXXL OCLV Carbon Tubular Front/Rear with SS washer (.82mm thick) 700C 622", 592.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 240064 RXXXL OCLV Carbon Tubular Front/Rear with SS washer (.82mm thick) 700C 622", 593.6, 0.0));
            rimlist.Add(new CRim("Bontrager PN 242752, R455 (04 Select Rd. extrusion) 700C 622", 596.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 243628 Race Lite OSB 29\" 700C 622", 607.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 243756 RXL Aero Clincher 650C 571", 547.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 250032 RXXXL OCLV Carbon Clincher Front w/SS washer (.82mm thick) 700C 622", 589.18, 0.0));
            rimlist.Add(new CRim("Bontrager PN 250033 RXXXL OCLV Carbon Clincher OSB Rear w/SS washer (.82mm thick) 700C 622", 589.18, 2.0));
            rimlist.Add(new CRim("Bontrager PN 981886 Clyde (24mm wide) 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 982951 Valiant symmetrical 26\" MTB 559", 539.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 984037 Maverick symmetrical 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 984038 Maverick OSB 26\" MTB 559", 545.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 984039 Mustang OSB 26\" MTB 559", 545.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 984042 Clyde 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 984238 Mustang symmetrical 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 984574 Valiant OSB 26\" MTB 559", 545.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 992115, 994103 Fairlane symmetric 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 992116, 994305 Fairlane OSB 700C 622", 607.0, 2.5));
            rimlist.Add(new CRim("Bontrager PN 992201Corvair symmetrical 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Bontrager PN 993524 Corvair OSB 26\" MTB 559", 545.0, 2.5));
            rimlist.Add(new CRim("Bontrager RaceLite Asym rear 26\" MTB 559", 545.0, 2.5));
            rimlist.Add(new CRim("Bontrager RaceLite front 26\" MTB 559", 542.0, 0.0));
            rimlist.Add(new CRim("Bontrager RXL Carbon Clincher Front PROTOTYPE 700C 622", 590.5, 0.0));
            rimlist.Add(new CRim("Bontrager RXL Carbon Clincher Rear OSB PROTOTYPE 700C 622", 593.5, 1.5));
            rimlist.Add(new CRim("Bontrager RXL OCLV Carbon Tubular Front/Rear Prototype 700C 622", 586.2, 0.0));
            rimlist.Add(new CRim("Bontrager TubeLess Disc, OSB, SSD=534.5+3mm for eyelettes 26\" MTB 559", 537.5, 2.5));
            rimlist.Add(new CRim("Bontrager, PN 221723, Mustang OSB, Tubeless 26\" MTB 559", 542.0, 2.5));
            rimlist.Add(new CRim("Bontrager, PN 231323, Mustang, Front, Tubeless, lightweight extrusion 26\" MTB 559", 539.0, 0.0));
            rimlist.Add(new CRim("Bontrager, PN 231327, Mustang, Disc, Tubeless 26\" MTB 559", 537.0, 2.5));
            rimlist.Add(new CRim("Bontrager/Mavic MA2, MA40  26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Bontrager/Weinmann BCR1 26\" MTB 559", 552.0, 0.0));
            rimlist.Add(new CRim("Breezer Backdraft  26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Aconcagua 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Alpha XL 26\" MTB 559", 553.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Amber  700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Atek 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Atlanta '96 (about 30mm tall) 700C 622", 575.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Barcelona '92 (12mm tall) 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Barcelona '92 (15mm tall) 700C 管胎专用", 609.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Beta 26\" MTB 559", 551.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Contax  26\" MTB 559", 539.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Delta (box section) 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Delta XL, Omega XL, Lambda, Omicron box (very low profile) 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Everest 26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Gamma box 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Helsinki '52 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Campagnolo K2  26\" MTB 559", 519.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Kappa 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Kilimangiaro 26\" MTB 559", 542.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Lambda (box section) 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Lambda \"V\" profile  (about 21mm tall) 700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Los Angeles '84  700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Melbourne '56 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Mexico '68 \"V\" profile (about 23mm tall) 700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Mirox 26\" MTB 559", 542.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Montreal '76  650C 571", 555.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Montreal '76 box style 700C 622", 608.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Montreal '76, new style (beveled semi-aero) 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Moskva '80  700C 622", 601.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Munchen '72 700C 622", 618.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Omega (box section) 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Omega \"V\" profile (about 21mm tall) 700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Omega 19  650C 571", 556.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Omega 19 box style (about 19mm wide) 700C 622", 608.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Omega Strada and Strada Hardox box style (about 20mm wide) 700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Record (box section) 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Roma '60 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Seoul '88  700C 管胎专用", 602.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Sigma (box section) 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Stheno 26\" MTB 559", 540.0, 0.0));
            rimlist.Add(new CRim("Campagnolo strada  650C 管胎专用", 568.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Strada  700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Campagnolo strada  700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Thorr  26\" MTB 559", 540.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Tokyo '64  700C 622", 609.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Topaz  700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Campagnolo V profile  700C 管胎专用", 603.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Victory (box section) 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Campagnolo XL Strada  700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Ypsilon \"V\" profile (about 21mm tall) 700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Ypsilon box 700C 622", 615.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Zark  26\" MTB 559", 543.0, 0.0));
            rimlist.Add(new CRim("Campagnolo Zeta 26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("da Vinci V22 http://www.davincitandems.com/ 700C 622", 596.5, 0.0));
            rimlist.Add(new CRim("DT Swiss E 540 Enduro disc 26\" MTB 559", 537.0, 0.0));
            rimlist.Add(new CRim("DT Swiss EX 5.1d (disc brakes) 26\" MTB 559", 539.0, 0.0));
            rimlist.Add(new CRim("DT Swiss FR 6.1d (disc brakes) 26\" MTB 559", 535.0, 0.0));
            rimlist.Add(new CRim("DT Swiss RR 1.1 700C 622", 599.0, 0.0));
            rimlist.Add(new CRim("DT Swiss TK 7.1 700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("DT Swiss X 450 MTB disc 26\" MTB 559", 540.0, 0.0));
            rimlist.Add(new CRim("DT Swiss X 450 MTB non disc 26\" MTB 559", 539.0, 0.0));
            rimlist.Add(new CRim("DT Swiss XR 4.1 (rim or disc brakes) 26\" MTB 559", 538.0, 0.0));
            rimlist.Add(new CRim("DT Swiss XR 4.1 ceramic 26\" MTB 559", 538.0, 0.0));
            rimlist.Add(new CRim("DT Swiss XR 4.1d (disc brakes) 26\" MTB 559", 538.0, 0.0));
            rimlist.Add(new CRim("Dunlop Special Lightweight 27\" 630", 623.0, 0.0));
            rimlist.Add(new CRim("Fiamme 71-Sport 27\" 630", 609.0, 0.0));
            rimlist.Add(new CRim("Fiamme 80-Elan 27\" 630", 603.0, 0.0));
            rimlist.Add(new CRim("Fiamme 80-Elan 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Fiamme Ergal (Yellow Label) 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Fiamme Ergal-Iride 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Fiamme Hard Silver 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Fiamme Master 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Fiamme Red Label 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Fiamme Speedy (track) 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Fiamme Super Corsa 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("FiR Alkor 650C 管胎专用", 565.0, 0.0));
            rimlist.Add(new CRim("FiR Alkor 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("FiR Antara 2000 650C 571", 511.0, 0.0));
            rimlist.Add(new CRim("FiR Antara 2000 650C 管胎专用", 511.0, 0.0));
            rimlist.Add(new CRim("FiR Antara 2000 700C 622", 562.0, 0.0));
            rimlist.Add(new CRim("FiR Antara 2000 700C 管胎专用", 562.0, 0.0));
            rimlist.Add(new CRim("FiR EA10  700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("FiR EA50 700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("FiR EA60  700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("FiR EA61 700C 622", 608.0, 0.0));
            rimlist.Add(new CRim("FiR EA65  700C 622", 609.0, 0.0));
            rimlist.Add(new CRim("FiR EL20 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("FiR EL45 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("FiR ES35 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("FiR EU90 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("FiR Isidis 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("FiR MS26, Downhill (same w/ machined sidewalls) 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("FiR MT 232 26\" MTB 559", 543.0, 0.0));
            rimlist.Add(new CRim("FiR MT 232 700C 622", 606.0, 0.0));
            rimlist.Add(new CRim("FiR Net97 (MEASURE TO BE SURE!) 700C 622", 585.0, 0.0));
            rimlist.Add(new CRim("FiR Net97 (MEASURE TO BE SURE!) 700C 622", 591.0, 0.0));
            rimlist.Add(new CRim("FiR Net97 700C 622", 585.0, 0.0));
            rimlist.Add(new CRim("FiR Pulsar 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("FiR Quasar 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("FiR SC150 actual weight 380gms! 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("FiR SC170 700C 622", 605.0, 0.0));
            rimlist.Add(new CRim("FiR SC200 700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("FiR SC350 700C 622", 582.0, 0.0));
            rimlist.Add(new CRim("FiR Sirus 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("FiR SRG30 (30mm deep aero rim, 440gms) 650C 571", 525.0, 0.0));
            rimlist.Add(new CRim("FiR SRG30 (30mm deep aero rim, 500gms) 700C 622", 582.0, 0.0));
            rimlist.Add(new CRim("FiR ST120 (400g) 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("FiR ST120L (340g) 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("FiR W 400 26\" MTB 559", 535.0, 0.0));
            rimlist.Add(new CRim("HJC 28孔双层圈 20X1.5 406", 392.0, 0.0));
            rimlist.Add(new CRim("IRD Cadence 700C 622", 604.0, 0.0));
            rimlist.Add(new CRim("IRD Cadence Aero (30 mm deep) 700C 622", 576.0, 0.0));
            rimlist.Add(new CRim("IRD Cadence VSR (virtual symmetry rim) 700C 622", 604.0, 1.5));
            rimlist.Add(new CRim("IRD Clyde 24 mm wide 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("IRD Tsunami 26\" MTB 559", 530.0, 0.0));
            rimlist.Add(new CRim("IRD Tsunami disk 26\" MTB 559", 528.0, 0.0));
            rimlist.Add(new CRim("K525 26\" MTB 559", 538.0, 0.0));
            rimlist.Add(new CRim("KINLIN NBR 406 20X1.5 406", 384.0, 0.0));
            rimlist.Add(new CRim("KINLIN NBR 451 20x1 1/8 451", 430.0, 0.0));
            rimlist.Add(new CRim("kinlin xr240（litepro 20寸406小刀圈） 20X1.5 406", 372.0, 0.0));
            rimlist.Add(new CRim("LEW Composite 650C 管胎专用", 522.0, 0.0));
            rimlist.Add(new CRim("LEW Composite 700C 管胎专用", 572.0, 0.0));
            rimlist.Add(new CRim("LITE PRO K-FUN 305 16x1.5 305", 285.0, 0.0));
            rimlist.Add(new CRim("LITE PRO K-PRO 451 20x1 1/8 451", 417.0, 0.0));
            rimlist.Add(new CRim("LITE PRO（kinlin代工）18寸355圈 18 355", 326.0, 0.0));
            rimlist.Add(new CRim("Lite-Pro 14寸小刀圈 14 ", 233.0, 0.0));
            rimlist.Add(new CRim("litePRO 16V 305 16x1.5 305", 287.0, 0.0));
            rimlist.Add(new CRim("Matrix 750 (22mm w x 20.5mm deep) 26\" MTB 559", 544.3, 0.0));
            rimlist.Add(new CRim("Matrix Aurora  27\" 630", 617.8, 0.0));
            rimlist.Add(new CRim("Matrix Aurora 700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("Matrix Aurora OSB (offset spoke bed) 700C 622", 606.0, 2.0));
            rimlist.Add(new CRim("Matrix Iso  700C 管胎专用", 611.0, 0.0));
            rimlist.Add(new CRim("Matrix ISO C 26\" MTB 559", 535.0, 0.0));
            rimlist.Add(new CRim("Matrix ISO C 27\" 630", 605.0, 0.0));
            rimlist.Add(new CRim("Matrix ISO C 700C 622", 597.0, 0.0));
            rimlist.Add(new CRim("Matrix ISO CII 26\" MTB 559", 539.0, 0.0));
            rimlist.Add(new CRim("Matrix ISO CII 27\" 630", 609.0, 0.0));
            rimlist.Add(new CRim("Matrix ISO CII 700C 622", 601.0, 0.0));
            rimlist.Add(new CRim("Matrix Lobo 26\" MTB 559", 551.0, 0.0));
            rimlist.Add(new CRim("Matrix Mt. Titan  26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Matrix PN 970704 ISO 3 700C 622", 595.0, 0.0));
            rimlist.Add(new CRim("Matrix PN 970704 ISO 3 700C 622", 595.0, 0.0));
            rimlist.Add(new CRim("Matrix PN 970704 ISO 3 700C 622", 595.0, 0.0));
            rimlist.Add(new CRim("Matrix PN 970704 ISO 3 700C 622", 603.9, 1.5));
            rimlist.Add(new CRim("Matrix PN 970704 ISO 3 700C 622", 603.9, 1.5));
            rimlist.Add(new CRim("Matrix PN 970704 ISO 3 700C 622", 603.9, 1.5));
            rimlist.Add(new CRim("Matrix S T Comp  26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Matrix Single Track 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Matrix Swami  26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Matrix Titan 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Matrix Titan 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Matrix Titan S 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Matrix Titan T (touring) 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Matrix Titan T (touring) 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Matrix Titan-II 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Mavic 117 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Mavic 121 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Mavic 192  700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Mavic 195 700C 622", 605.0, 0.0));
            rimlist.Add(new CRim("Mavic 217 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 539.0, 0.0));
            rimlist.Add(new CRim("Mavic 217D 26\" MTB 559", 540.5, 0.0));
            rimlist.Add(new CRim("Mavic 220 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 543.0, 0.0));
            rimlist.Add(new CRim("Mavic 221 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 543.5, 0.0));
            rimlist.Add(new CRim("Mavic 238 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 544.5, 0.0));
            rimlist.Add(new CRim("Mavic 238N 26\" MTB 559", 543.0, 0.0));
            rimlist.Add(new CRim("Mavic A 119 700C 622", 604.0, 0.0));
            rimlist.Add(new CRim("Mavic A 317 Disc 700C 622", 602.5, 0.0));
            rimlist.Add(new CRim("Mavic A 319 D 05 700C 622", 602.5, 0.0));
            rimlist.Add(new CRim("Mavic A 319 D 700C 622", 604.5, 0.0));
            rimlist.Add(new CRim("Mavic A 719 700C 622", 600.5, 0.0));
            rimlist.Add(new CRim("Mavic Argent 10 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Mavic Argent 12 SSC 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Mavic Argent 7 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Mavic Argent 8 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Mavic Bleu SSC 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Mavic CX18  24\" 管胎专用", 517.0, 0.0));
            rimlist.Add(new CRim("Mavic CX18  650C 管胎专用", 577.0, 0.0));
            rimlist.Add(new CRim("Mavic CX18 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP10  700C 622", 600.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP10  700C 622", 601.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP11 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 601.5, 0.0));
            rimlist.Add(new CRim("Mavic CXP12 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 650C 571", 549.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP12 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 597.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP14 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 650C 571", 539.5, 0.0));
            rimlist.Add(new CRim("Mavic CXP14 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 587.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP14 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) MEASURE TO BE SURE!! 650C 571", 536.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP14 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) MEASURE TO BE SURE!! 650C 571", 540.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP21 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 601.5, 0.0));
            rimlist.Add(new CRim("Mavic CXP22N 700C 622", 598.5, 0.0));
            rimlist.Add(new CRim("Mavic CXP22S 700C 622", 599.5, 0.0));
            rimlist.Add(new CRim("Mavic CXP23 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 598.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP30 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 587.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP30 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 管胎专用", 587.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP33 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples)  650C 571", 550.0, 0.0));
            rimlist.Add(new CRim("Mavic CXP33 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 598.0, 0.0));
            rimlist.Add(new CRim("Mavic D3.1 26\" MTB 559", 534.5, 0.0));
            rimlist.Add(new CRim("Mavic D321 Disc (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 537.5, 0.0));
            rimlist.Add(new CRim("Mavic D521 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 535.0, 0.0));
            rimlist.Add(new CRim("Mavic E2 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Mavic E2, G40, Mod E 27\" 630", 620.0, 0.0));
            rimlist.Add(new CRim("Mavic Energie M217 26\" MTB 559", 540.0, 0.0));
            rimlist.Add(new CRim("Mavic Energie M7 26\" MTB 559", 540.0, 0.0));
            rimlist.Add(new CRim("Mavic EX 325 Disc 26\" MTB 559", 536.5, 0.0));
            rimlist.Add(new CRim("Mavic EX 721 26\" MTB 559", 534.5, 0.0));
            rimlist.Add(new CRim("Mavic EX 729 Disc 26\" MTB 559", 537.0, 0.0));
            rimlist.Add(new CRim("Mavic EX 823 Disc 05 26\" MTB 559", 530.5, 0.0));
            rimlist.Add(new CRim("Mavic EX 823 Disc 26\" MTB 559", 534.5, 0.0));
            rimlist.Add(new CRim("Mavic F 219 Disc 26\" MTB 559", 538.5, 0.0));
            rimlist.Add(new CRim("Mavic F519 26\" MTB 559", 538.5, 0.0));
            rimlist.Add(new CRim("Mavic F519 26\" MTB 559", 543.0, 0.0));
            rimlist.Add(new CRim("Mavic G40 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Mavic GEL280 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Mavic GL330  24\" 管胎专用", 517.0, 0.0));
            rimlist.Add(new CRim("Mavic GL330  650C 管胎专用", 576.0, 0.0));
            rimlist.Add(new CRim("Mavic GL330 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Mavic GP4 new angular (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 管胎专用", 612.0, 0.0));
            rimlist.Add(new CRim("Mavic GP4 old rounder 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Mavic M230  26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Mavic M231 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Mavic M238 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Mavic M261 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Mavic M281  26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Mavic MA 700C 622", 612.5, 0.0));
            rimlist.Add(new CRim("Mavic MA single eyelets 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Mavic MA2 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Mavic MA2 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Mavic MA3 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Mavic MA3 700C 622", 607.5, 0.0));
            rimlist.Add(new CRim("Mavic MA40  650C 571", 559.0, 0.0));
            rimlist.Add(new CRim("Mavic MA40 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Mavic MA40 26\" MTB 559", 550.0, 0.0));
            rimlist.Add(new CRim("Mavic MA40 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Mavic Mach 2  24\" 管胎专用", 504.0, 0.0));
            rimlist.Add(new CRim("Mavic Mach 2  650C 管胎专用", 563.0, 0.0));
            rimlist.Add(new CRim("Mavic Mach 2CD2  650C 管胎专用", 557.0, 0.0));
            rimlist.Add(new CRim("Mavic Mach 2CD2  700C 管胎专用", 604.0, 0.0));
            rimlist.Add(new CRim("Mavic Mach2 700C 管胎专用", 603.0, 0.0));
            rimlist.Add(new CRim("Mavic Mod 3 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Mavic Mod 3 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Mavic Mod 3D Argent 700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Mavic Mod 4 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Mavic Mod 4 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Mavic Mod 50 700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Mavic Mod E 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Mavic Module 3, Module 4 ArgentD 650B 584", 572.0, 0.0));
            rimlist.Add(new CRim("Mavic Montlhery Pro 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Mavic Open 20  700C 622", 601.0, 0.0));
            rimlist.Add(new CRim("Mavic Open 20  700C 622", 602.0, 0.0));
            rimlist.Add(new CRim("Mavic Open 20D  700C 622", 602.0, 0.0));
            rimlist.Add(new CRim("Mavic Open 20D  700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Mavic Open 4 CD  24x1 1/8 520", 500.0, 0.0));
            rimlist.Add(new CRim("Mavic Open 4CD 650C 571", 554.0, 0.0));
            rimlist.Add(new CRim("Mavic Open 4CD 700C 622", 608.0, 0.0));
            rimlist.Add(new CRim("Mavic Open Pro (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 650C 571", 554.0, 0.0));
            rimlist.Add(new CRim("Mavic Open Pro (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 605.0, 0.0));
            rimlist.Add(new CRim("Mavic Open SUP (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Mavic OR7 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Mavic Oxygen M6  26\" MTB 559", 542.0, 0.0));
            rimlist.Add(new CRim("Mavic Paris Roubaix SSC (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 管胎专用", 615.0, 0.0));
            rimlist.Add(new CRim("Mavic Piste (track) 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Mavic Rando M4 26\" MTB 559", 550.0, 0.0));
            rimlist.Add(new CRim("Mavic Rando M5 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Mavic Reflex -- MEASURE TO BE SURE!!! 700C 管胎专用", 613.0, 0.0));
            rimlist.Add(new CRim("Mavic Reflex -- MEASURE TO BE SURE!!! 700C 管胎专用", 613.0, 0.0));
            rimlist.Add(new CRim("Mavic Reflex -- MEASURE TO BE SURE!!! 700C 管胎专用", 613.0, 0.0));
            rimlist.Add(new CRim("Mavic Reflex -- MEASURE TO BE SURE!!! 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Mavic Reflex -- MEASURE TO BE SURE!!! 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Mavic Reflex -- MEASURE TO BE SURE!!! 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Mavic Reflex (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 605.0, 0.0));
            rimlist.Add(new CRim("Mavic T138 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Mavic T215 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 609.0, 0.0));
            rimlist.Add(new CRim("Mavic T217 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Mavic T221 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 605.5, 0.0));
            rimlist.Add(new CRim("Mavic T223 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Mavic T224 700C 622", 604.5, 0.0));
            rimlist.Add(new CRim("Mavic T238 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 606.5, 0.0));
            rimlist.Add(new CRim("Mavic T261 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Mavic T519 26\" MTB 559", 540.2, 0.0));
            rimlist.Add(new CRim("Mavic T519 700C 622", 601.5, 0.0));
            rimlist.Add(new CRim("Mavic T520 700C 622", 600.5, 0.0));
            rimlist.Add(new CRim("Mavic X138 eyelets (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 543.0, 0.0));
            rimlist.Add(new CRim("Mavic X138N no eyelets (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 543.0, 0.0));
            rimlist.Add(new CRim("Mavic X139N no eyelets 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Mavic X221 eyelets (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 543.5, 0.0));
            rimlist.Add(new CRim("Mavic X221N no eyelets (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 542.5, 0.0));
            rimlist.Add(new CRim("Mavic X222 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 542.5, 0.0));
            rimlist.Add(new CRim("Mavic X223 Disc (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 539.0, 0.0));
            rimlist.Add(new CRim("Mavic X225 26\" MTB 559", 540.5, 0.0));
            rimlist.Add(new CRim("Mavic X3.1 26\" MTB 559", 534.5, 0.0));
            rimlist.Add(new CRim("Mavic X3.1 Disc 26\" MTB 559", 534.5, 0.0));
            rimlist.Add(new CRim("Mavic X3.1 UST disc specific. 16 to 18mm nipples required. 26\" MTB 559", 532.0, 0.0));
            rimlist.Add(new CRim("Mavic X317 Disc (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 541.5, 0.0));
            rimlist.Add(new CRim("Mavic X517 (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 540.5, 0.0));
            rimlist.Add(new CRim("Mavic X618 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Mavic XC 317 Disc 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Mavic XC 717 Disc 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Mavic XC717 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Mavic XM 117 26\" MTB 559", 542.5, 0.0));
            rimlist.Add(new CRim("Mavic XM 117 Disc 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Mavic XM 317 Disc 26\" MTB 559", 538.5, 0.0));
            rimlist.Add(new CRim("Mavic XM 321 Disc 26\" MTB 559", 538.5, 0.0));
            rimlist.Add(new CRim("Mavic XM 517 26\" MTB 559", 540.5, 0.0));
            rimlist.Add(new CRim("Mavic XM 719 26\" MTB 559", 538.5, 0.0));
            rimlist.Add(new CRim("Mavic XM 819 26\" MTB 559", 534.5, 0.0));
            rimlist.Add(new CRim("Mavic XM 819 Disc 26\" MTB 559", 534.5, 0.0));
            rimlist.Add(new CRim("Mavic Xx (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 20\" BMX 406", 399.0, 0.0));
            rimlist.Add(new CRim("Mavic Xy (ERD is Mavic's Nipple Seat Dia + 3mm for nipples) 20\" BMX 406", 392.0, 0.0));
            rimlist.Add(new CRim("Mavic XY 24\" BMX 507", 492.5, 0.0));
            rimlist.Add(new CRim("Mavic XZ 20\" BMX 406", 387.5, 0.0));
            rimlist.Add(new CRim("MOTACHIE(朦太奇)RM-DA20-16 16x1.5 305", 287.0, 0.0));
            rimlist.Add(new CRim("Moulton, Alex (factory rim) 17x1 1/4 369", 363.7, 0.0));
            rimlist.Add(new CRim("Nashbar UC13 700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Nisi AN-85 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Nisi Countach 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("Nisi G-27 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Nisi Mixer 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Nisi Sludi Mod 290 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Nisi Sludi Mod 320 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Nisi Solidal 700C 管胎专用", 617.0, 0.0));
            rimlist.Add(new CRim("NOTUBES ZTR 355 26\" MTB 559", 540.0, 0.0));
            rimlist.Add(new CRim("NOTUBES ZTR 355 29'er 700C 622", 604.0, 0.0));
            rimlist.Add(new CRim("NOTUBES ZTR Freeride 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("NOTUBES ZTR Olympic 26\" MTB 559", 536.0, 0.0));
            rimlist.Add(new CRim("NOTUBES ZTR Olympic 29'er 700C 622", 417.0, 0.0));
            rimlist.Add(new CRim("oyama 20寸车用单层圈（以M300用的为例） 20X1.5 406", 395.0, 0.0));
            rimlist.Add(new CRim("Oyama16寸用的单层菜圈 16x1.5 305", 295.0, 0.0));
            rimlist.Add(new CRim("Performance A19T  700C 管胎专用", 603.0, 0.0));
            rimlist.Add(new CRim("Performance C19  700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("Performance C19SL  700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Performance TR22  27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("RAINBOW DA20 406小刀圈 20X1.5 406", 386.0, 0.0));
            rimlist.Add(new CRim("Rigida 1320  27\" 630", 617.0, 0.0));
            rimlist.Add(new CRim("Rigida 1320  700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("Rigida ARIES (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Rigida ARIES (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 605.0, 0.0));
            rimlist.Add(new CRim("Rigida AS 23 X (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 24x1 3/8 540", 531.1, 0.0));
            rimlist.Add(new CRim("Rigida AS 23 X (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 27\" 630", 621.2, 0.0));
            rimlist.Add(new CRim("Rigida AS 23 X (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 500A 440", 431.3, 0.0));
            rimlist.Add(new CRim("Rigida AS 23 X (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 550A 490", 481.0, 0.0));
            rimlist.Add(new CRim("Rigida AS 23 X (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 600A 540", 532.2, 0.0));
            rimlist.Add(new CRim("Rigida AS 23 X (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 650A 590", 581.0, 0.0));
            rimlist.Add(new CRim("Rigida AS 23 X (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 613.7, 0.0));
            rimlist.Add(new CRim("Rigida AS 26 F (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 548.4, 0.0));
            rimlist.Add(new CRim("Rigida AS 26 F (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 650A 590", 497.0, 0.0));
            rimlist.Add(new CRim("Rigida AS 26 F (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 650B 584", 573.5, 0.0));
            rimlist.Add(new CRim("Rigida AS 26 F (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Rigida AS 26 FL (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 20\" BMX 406", 395.2, 0.0));
            rimlist.Add(new CRim("Rigida AS 26 FL (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 20x1 1/8 451", 433.0, 0.0));
            rimlist.Add(new CRim("Rigida AS 26 FL (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 24\" BMX 507", 497.0, 0.0));
            rimlist.Add(new CRim("Rigida AS 26 FL (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 548.4, 0.0));
            rimlist.Add(new CRim("Rigida AS 26 FL (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 400A 340", 329.3, 0.0));
            rimlist.Add(new CRim("Rigida CHP-6 700C 622", 602.0, 0.0));
            rimlist.Add(new CRim("Rigida CHRINA (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 608.0, 0.0));
            rimlist.Add(new CRim("Rigida DH 30 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 24\" BMX 507", 487.4, 0.0));
            rimlist.Add(new CRim("Rigida DH 30 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 539.9, 0.0));
            rimlist.Add(new CRim("Rigida DP 18 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 581.0, 0.0));
            rimlist.Add(new CRim("Rigida DP 18 650C 571", 530.7, 0.0));
            rimlist.Add(new CRim("Rigida DP 2000 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 521.0, 0.0));
            rimlist.Add(new CRim("Rigida DP 2000 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 584.3, 0.0));
            rimlist.Add(new CRim("Rigida DP 22 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 519.6, 0.0));
            rimlist.Add(new CRim("Rigida DP 25 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 532.0, 0.0));
            rimlist.Add(new CRim("Rigida DP 25 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 595.6, 0.0));
            rimlist.Add(new CRim("Rigida DPX (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 592.4, 0.0));
            rimlist.Add(new CRim("Rigida EXCEL (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 608.0, 0.0));
            rimlist.Add(new CRim("Rigida GENIUS (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 534.0, 0.0));
            rimlist.Add(new CRim("Rigida GRIFFIN (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 544.1, 0.0));
            rimlist.Add(new CRim("Rigida GRIZZLY (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 539.6, 0.0));
            rimlist.Add(new CRim("Rigida HLC2000  700C 622", 606.0, 0.0));
            rimlist.Add(new CRim("Rigida LIBRA (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 547.4, 0.0));
            rimlist.Add(new CRim("Rigida LIBRA (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 610.7, 0.0));
            rimlist.Add(new CRim("Rigida M 19 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 16x1.75 305", 295.0, 0.0));
            rimlist.Add(new CRim("Rigida M 19 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 20\" BMX 406", 395.9, 0.0));
            rimlist.Add(new CRim("Rigida M 19 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 24\" BMX 507", 497.8, 0.0));
            rimlist.Add(new CRim("Rigida M 19 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Rigida M 19 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 612.5, 0.0));
            rimlist.Add(new CRim("Rigida M 21 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 16x1.75 305", 295.0, 0.0));
            rimlist.Add(new CRim("Rigida M 21 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 20\" BMX 406", 395.9, 0.0));
            rimlist.Add(new CRim("Rigida M 21 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 24\" BMX 507", 497.8, 0.0));
            rimlist.Add(new CRim("Rigida M 21 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Rigida M 21 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 612.5, 0.0));
            rimlist.Add(new CRim("Rigida MENSA (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 547.1, 0.0));
            rimlist.Add(new CRim("Rigida NORMA (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 547.1, 0.0));
            rimlist.Add(new CRim("Rigida NOVA (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 600.8, 0.0));
            rimlist.Add(new CRim("Rigida NOVA R (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 650C 571", 551.7, 0.0));
            rimlist.Add(new CRim("Rigida OLYMPUS (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 531.9, 0.0));
            rimlist.Add(new CRim("Rigida OLYMPUS (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 592.4, 0.0));
            rimlist.Add(new CRim("Rigida ORION (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 547.4, 0.0));
            rimlist.Add(new CRim("Rigida ORION/MENSA (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 610.7, 0.0));
            rimlist.Add(new CRim("Rigida PHOENIX (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Rigida PHOENIX (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 604.8, 0.0));
            rimlist.Add(new CRim("Rigida Score  700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Rigida SHC (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Rigida SHP (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Rigida SIRIUS (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 547.4, 0.0));
            rimlist.Add(new CRim("Rigida SIRIUS (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 610.7, 0.0));
            rimlist.Add(new CRim("Rigida SLP (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Rigida steel 26x1 3/8 26x1 3/8 590", 582.0, 0.0));
            rimlist.Add(new CRim("Rigida steel Superchromix 1.25  27\" 630", 621.0, 0.0));
            rimlist.Add(new CRim("Rigida steel Superchromix 1.75  26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Rigida TAURUS (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 543.4, 0.0));
            rimlist.Add(new CRim("Rigida TAURUS 2000 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 540.1, 0.0));
            rimlist.Add(new CRim("Rigida TUB 25 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 539.9, 0.0));
            rimlist.Add(new CRim("Rigida TUB 25/ TUB 26 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 604.3, 0.0));
            rimlist.Add(new CRim("Rigida TUB 26 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 539.9, 0.0));
            rimlist.Add(new CRim("Rigida TUCANA (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 543.2, 0.0));
            rimlist.Add(new CRim("Rigida TURBO (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 537.2, 0.0));
            rimlist.Add(new CRim("Rigida VELA (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Rigida X PLORER (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 20\" BMX 406", 376.0, 0.0));
            rimlist.Add(new CRim("Rigida X PLORER (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 529.4, 0.0));
            rimlist.Add(new CRim("Rigida X PLORER (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 592.4, 0.0));
            rimlist.Add(new CRim("Rigida XC 420 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 543.8, 0.0));
            rimlist.Add(new CRim("Rigida ZAC 2000 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 599.8, 0.0));
            rimlist.Add(new CRim("Rigida ZAC 2000+D574 26\" MTB 559", 536.5, 0.0));
            rimlist.Add(new CRim("Rigida/Weinmann ZAC 19 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Rigida/Weinmann ZAC 19 (ERD is Rigida's Nipple Seat Dia + 3mm for nipples) 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Rinard/Hollenbeck carbon clincher proto #1 700C 622", 587.0, 0.0));
            rimlist.Add(new CRim("Ritchey Aero Road Comp centered holes (ERD is nipple contact dia + 3mm for nipples) 700C 622", 602.0, 0.0));
            rimlist.Add(new CRim("Ritchey Aero Road Comp OCR (ERD is nipple contact dia + 3mm for nipples) 700C 622", 602.0, 2.5));
            rimlist.Add(new CRim("Ritchey Aero Road Pro centered holes (ERD is nipple contact dia + 3mm for nipples) 700C 622", 602.0, 0.0));
            rimlist.Add(new CRim("Ritchey Aero Road Pro OCR (ERD is nipple contact dia + 3mm for nipples) 700C 622", 602.0, 2.5));
            rimlist.Add(new CRim("Ritchey Aero Road WCS centered holes (ERD is nipple contact dia + 3mm for nipples) 700C 622", 602.0, 0.0));
            rimlist.Add(new CRim("Ritchey Aero Road WCS OCR (ERD is nipple contact dia + 3mm for nipples) 700C 622", 602.0, 2.5));
            rimlist.Add(new CRim("Ritchey Disc OCR (ERD is Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 546.0, 2.5));
            rimlist.Add(new CRim("Ritchey OCR Comp (ERD is Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 545.0, 2.5));
            rimlist.Add(new CRim("Ritchey OCR Pro (ERD is Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 545.0, 2.5));
            rimlist.Add(new CRim("Ritchey OCR WCS (ERD is Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 545.0, 2.5));
            rimlist.Add(new CRim("Ritchey Rock 395 26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Ritchey Rock 415 26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Ritchey Rock 440SC  26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Ritchey Rock 450CE 700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("Ritchey Rock 490 700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("Ritchey Rock 490C 700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("Ritchey Rock 700 Pro (ERD is Nipple Seat Dia + 3mm for nipples) 700C 622", 605.0, 0.0));
            rimlist.Add(new CRim("Ritchey Rock Comp (ERD is Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Ritchey Rock Pro (ERD is Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Ritchey Rock WCS (ERD is Nipple Seat Dia + 3mm for nipples) 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Ritchey Trekking Comp 700C 622", 596.0, 0.0));
            rimlist.Add(new CRim("Ritchey Trekking Comp OCR 700C 622", 594.0, 2.5));
            rimlist.Add(new CRim("Ritchey Vantage Comp 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Ritchey Vantage Comp 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Ritchey Vantage Cross-Sport  26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Ritchey Vantage Cross-Sport  700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Ritchey Vantage Expert  26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Ritchey Vantage Pro  26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Ritchey Vantage Sport 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Rolf Sestriere 700C 622", 595.0, 0.0));
            rimlist.Add(new CRim("Rolf Vector 700C 622", 595.0, 0.0));
            rimlist.Add(new CRim("Rolf Vector Carbon, Lew made. For Greer internal nipples + 2.5mm washers 650C 管胎专用", 530.4, 0.0));
            rimlist.Add(new CRim("Rolf Vector Comp, Trek made 700C 622", 580.0, 0.0));
            rimlist.Add(new CRim("Rolf Vector Pro, Araya made. For Greer internal nipples + 4mm washers 650C 管胎专用", 553.0, 0.0));
            rimlist.Add(new CRim("Rolf Vector Pro, Araya made. For Greer internal nipples + 4mm washers 700C 622", 604.1, 0.0));
            rimlist.Add(new CRim("Rolf Vector Pro, Araya made. For Greer internal nipples + 4mm washers 700C 管胎专用", 608.1, 0.0));
            rimlist.Add(new CRim("Rolf Vector Pro, Araya rim, including 9mm Greer nipple + 4mm washer 650C 571", 551.7, 0.0));
            rimlist.Add(new CRim("Rolf Vector Pro, Trek made. For Greer internal nipples, no washers 650C 571", 546.0, 0.0));
            rimlist.Add(new CRim("Rolf Vector Pro, Trek made. For Greer internal nipples, no washers 650C 管胎专用", 553.0, 0.0));
            rimlist.Add(new CRim("Rolf Vector Pro, Trek made. For Greer internal nipples, no washers 700C 622", 598.0, 0.0));
            rimlist.Add(new CRim("Rolf Vector Pro, Trek made. For Greer internal nipples, no washers 700C 管胎专用", 603.7, 0.0));
            rimlist.Add(new CRim("Saavedra aero 650C 管胎专用", 558.0, 0.0));
            rimlist.Add(new CRim("Saavedra aero 700C 管胎专用", 608.0, 0.0));
            rimlist.Add(new CRim("Salsa Delgado 22.5mm wide 26\" MTB 559", 543.0, 0.0));
            rimlist.Add(new CRim("Salsa Delgado Cross 22.5w x 18h 700C 622", 604.0, 0.0));
            rimlist.Add(new CRim("Salsa Gordo 27mm wide 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Salsa Semi 24mm wide 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Saturae:  All greay anodized models 700C 管胎专用", 618.0, 0.0));
            rimlist.Add(new CRim("Snow Cat ASYM (44mm wide) http://www.allweathersports.com/winter/snowcats.html 26\" MTB 559", 554.0, 8.8));
            rimlist.Add(new CRim("Snow Cat SYM (44mm wide) http://www.allweathersports.com/winter/snowcats.html 26\" MTB 559", 554.0, 0.0));
            rimlist.Add(new CRim("Specialized C20 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Specialized C22 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Specialized GX23 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Specialized GX26 26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Specialized HC19 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Specialized HC20 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Specialized Saturae C20 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Specialized Saturae C22 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Specialized Saturae HC19 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Specialized Saturne HX22 26\" MTB 559", 555.0, 0.0));
            rimlist.Add(new CRim("Specialized Saturne HX28 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Specialized Saturne HX32 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Specialized Saturne X22 26\" MTB 559", 555.0, 0.0));
            rimlist.Add(new CRim("Specialized Saturne X28 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Specialized Saturne X32 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Specialized X26 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Specialized XL21 26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Specialized XL23  26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Specialized Z21  26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Sun 0 degree Lite 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Sun 0 degree XC 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Sun 0 degree XC 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Sun Ascent 650C 571", 552.0, 0.0));
            rimlist.Add(new CRim("Sun Ascent 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Sun AT18  20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Sun AT18  26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Sun AT18  700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun Big City 20\" BMX 406", 389.0, 0.0));
            rimlist.Add(new CRim("Sun Buzz Clincher (carbon, alu brake track) 700C 622", 528.0, 0.0));
            rimlist.Add(new CRim("Sun Buzz MTB 26\" MTB 559", 471.0, 0.0));
            rimlist.Add(new CRim("Sun Buzz Road (carbon, alu brake track) 650C 管胎专用", 507.0, 0.0));
            rimlist.Add(new CRim("Sun Buzz Road (carbon, alu brake track) 700C 管胎专用", 556.0, 0.0));
            rimlist.Add(new CRim("Sun Buzz Special Event (carbon, no brake track) 650C 管胎专用", 507.0, 0.0));
            rimlist.Add(new CRim("Sun Buzz Special Event (carbon, no brake track) 700C 管胎专用", 556.0, 0.0));
            rimlist.Add(new CRim("Sun C16 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Sun C16 27\" 630", 619.0, 0.0));
            rimlist.Add(new CRim("Sun C20 24x1 3/8 540", 528.0, 0.0));
            rimlist.Add(new CRim("Sun C20 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Sun C20 tandem  26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Sun CR16 20\" BMX 406", 393.0, 0.0));
            rimlist.Add(new CRim("Sun CR16 20x1 1/8 451", 439.0, 0.0));
            rimlist.Add(new CRim("Sun CR16 24\" BMX 507", 494.0, 0.0));
            rimlist.Add(new CRim("Sun CR16 24x1 3/8 540", 527.0, 0.0));
            rimlist.Add(new CRim("Sun CR16 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Sun CR16 27\" 630", 615.0, 0.0));
            rimlist.Add(new CRim("Sun CR16 700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("Sun CR17A 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Sun CR17A 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Sun CR18, single eyelets 20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Sun CR18, single eyelets 20x1 1/8 451", 438.0, 0.0));
            rimlist.Add(new CRim("Sun CR18, single eyelets 24\" BMX 507", 493.0, 0.0));
            rimlist.Add(new CRim("Sun CR18, single eyelets 24x1 3/8 540", 532.0, 0.0));
            rimlist.Add(new CRim("Sun CR18, single eyelets 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Sun CR18, single eyelets 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun CR20  20\" BMX 406", 392.0, 0.0));
            rimlist.Add(new CRim("Sun CR20  20x1 1/8 451", 435.0, 0.0));
            rimlist.Add(new CRim("Sun CR20  24\" BMX 507", 493.0, 0.0));
            rimlist.Add(new CRim("Sun CR20  24x1 3/8 540", 529.0, 0.0));
            rimlist.Add(new CRim("Sun CR20  26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Sun CR20  26x1 3/8 590", 576.0, 0.0));
            rimlist.Add(new CRim("Sun CR20  700C 622", 608.0, 0.0));
            rimlist.Add(new CRim("Sun CRE16 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Sun CRE16 27\" 630", 617.0, 0.0));
            rimlist.Add(new CRim("Sun CRE16 700C 622", 609.0, 0.0));
            rimlist.Add(new CRim("Sun CRE16, CRT16II 20\" BMX 406", 390.0, 0.0));
            rimlist.Add(new CRim("Sun CRE16, CRT16II 24\" BMX 507", 493.0, 0.0));
            rimlist.Add(new CRim("Sun CRT16 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Sun CRT16II 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Sun CRT16II 27\" 630", 617.0, 0.0));
            rimlist.Add(new CRim("Sun CRT16II 700C 622", 609.0, 0.0));
            rimlist.Add(new CRim("Sun CRT17A 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Sun D253 27\" 630", 619.0, 0.0));
            rimlist.Add(new CRim("Sun DoubleWide 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Sun DS1-XC 24\" BMX 507", 492.0, 0.0));
            rimlist.Add(new CRim("Sun DS1-XC 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Sun ICI-1 20x1 1/8 451", 438.0, 0.0));
            rimlist.Add(new CRim("Sun ICI-1 24\" BMX 507", 507.0, 0.0));
            rimlist.Add(new CRim("Sun L13 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Sun L13 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Sun L17 20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Sun L17 20x1 1/8 451", 441.0, 0.0));
            rimlist.Add(new CRim("Sun L17 24x1 3/8 540", 530.0, 0.0));
            rimlist.Add(new CRim("Sun L17 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun L17, L20 27\" 630", 620.0, 0.0));
            rimlist.Add(new CRim("Sun L18 20\" BMX 406", 390.0, 0.0));
            rimlist.Add(new CRim("Sun L18 24\" BMX 507", 493.0, 0.0));
            rimlist.Add(new CRim("Sun L18 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Sun L18 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun L20 20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Sun L20 20x1 1/8 451", 441.0, 0.0));
            rimlist.Add(new CRim("Sun L20 24\" BMX 507", 497.0, 0.0));
            rimlist.Add(new CRim("Sun L20 24x1 3/8 540", 529.0, 0.0));
            rimlist.Add(new CRim("Sun L20 26x1 3/8 590", 578.0, 0.0));
            rimlist.Add(new CRim("Sun L20 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun L20 Style I 26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Sun L25 26\" MTB 559", 550.0, 0.0));
            rimlist.Add(new CRim("Sun L25 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun L25, M25 20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Sun L25, M25 24\" BMX 507", 497.0, 0.0));
            rimlist.Add(new CRim("Sun L25, M25 24x1 1/8 520", 507.0, 0.0));
            rimlist.Add(new CRim("Sun M13 27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Sun M13 700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("Sun M13II 20x1 1/8 451", 440.0, 0.0));
            rimlist.Add(new CRim("Sun M13II 24x1 1/8 520", 509.0, 0.0));
            rimlist.Add(new CRim("Sun M13II 24x1 3/8 540", 530.0, 0.0));
            rimlist.Add(new CRim("Sun M13II 27\" 630", 619.0, 0.0));
            rimlist.Add(new CRim("Sun M13II 650C 571", 560.0, 0.0));
            rimlist.Add(new CRim("Sun M13II 700C 622", 610.0, 0.0));
            rimlist.Add(new CRim("Sun M13L  20x1 1/8 451", 441.0, 0.0));
            rimlist.Add(new CRim("Sun M13L  24x1 1/8 520", 510.0, 0.0));
            rimlist.Add(new CRim("Sun M13L  24x1 3/8 540", 528.0, 0.0));
            rimlist.Add(new CRim("Sun M13L  26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Sun M13L  27\" 630", 618.0, 0.0));
            rimlist.Add(new CRim("Sun M13L  650C 571", 561.0, 0.0));
            rimlist.Add(new CRim("Sun M13L  700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun M14A (old) 27\" 630", 614.0, 0.0));
            rimlist.Add(new CRim("Sun M14A (old) 700C 622", 606.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 16x1 3/8 349", 330.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 20x1 1/8 451", 432.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 24\" BMX 507", 491.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 24x1 1/8 520", 502.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 24x1 3/8 540", 522.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 26\" MTB 559", 539.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 26x1 3/8 590", 570.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 27\" 630", 611.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 650C 571", 552.0, 0.0));
            rimlist.Add(new CRim("Sun M14A 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Sun M17  20x1 1/8 451", 440.0, 0.0));
            rimlist.Add(new CRim("Sun M17 27\" 630", 620.0, 0.0));
            rimlist.Add(new CRim("Sun M17 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun M17 Moulton 17x1 1/4 369", 364.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (582 mm OD) 650C 管胎专用", 560.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 20\" 管胎专用", 411.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 20\" 管胎专用", 411.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 20\" 管胎专用", 411.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 20\" 管胎专用", 441.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 20\" 管胎专用", 441.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 20\" 管胎专用", 441.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 24\" 管胎专用", 509.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 24\" 管胎专用", 509.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 24\" 管胎专用", 509.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 24\" 管胎专用", 512.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 24\" 管胎专用", 512.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (MEASURE TO BE SURE!) 24\" 管胎专用", 512.0, 0.0));
            rimlist.Add(new CRim("Sun M17A (tubular) 700C 管胎专用", 613.0, 0.0));
            rimlist.Add(new CRim("Sun M19A  24\" 管胎专用", 502.0, 0.0));
            rimlist.Add(new CRim("Sun M19A 700C 管胎专用", 604.0, 0.0));
            rimlist.Add(new CRim("Sun M19A, M19AII (331 mm OD) 16\" 管胎专用", 298.0, 0.0));
            rimlist.Add(new CRim("Sun M19A, M19AII (382 mm OD) 18\" 管胎专用", 352.0, 0.0));
            rimlist.Add(new CRim("Sun M19A, M19AII (432 mm OD) 20\" 管胎专用", 402.0, 0.0));
            rimlist.Add(new CRim("Sun M19A, M19AII (582 mm OD) 650C 管胎专用", 552.0, 0.0));
            rimlist.Add(new CRim("Sun M19AII (532 mm OD) 24\" 管胎专用", 503.0, 0.0));
            rimlist.Add(new CRim("Sun M19AII (632 mm OD) 700C 管胎专用", 602.0, 0.0));
            rimlist.Add(new CRim("Sun M20 20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Sun M20 27\" 630", 620.0, 0.0));
            rimlist.Add(new CRim("Sun M20 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Sun M20A 27\" 630", 619.0, 0.0));
            rimlist.Add(new CRim("Sun M20B  700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun M20B (582 mm OD) 650C 管胎专用", 560.0, 0.0));
            rimlist.Add(new CRim("Sun M20B 700C 管胎专用", 614.0, 0.0));
            rimlist.Add(new CRim("Sun M25  20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Sun M25  24\" BMX 507", 497.0, 0.0));
            rimlist.Add(new CRim("Sun M25  24x1 1/8 520", 507.0, 0.0));
            rimlist.Add(new CRim("Sun M25  27\" 630", 619.0, 0.0));
            rimlist.Add(new CRim("Sun M25  700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun M25 26\" MTB 559", 550.0, 0.0));
            rimlist.Add(new CRim("Sun M25 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun Mammoth BFR (Big Fat Rim) 20\" BMX 406", 394.0, 0.0));
            rimlist.Add(new CRim("Sun Mammoth BFR (Big Fat Rim) 24\" BMX 507", 492.0, 0.0));
            rimlist.Add(new CRim("Sun Mammoth BFR (Big Fat Rim) 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Sun ME14A 20x1 1/8 451", 426.0, 0.0));
            rimlist.Add(new CRim("Sun ME14A 24x1 1/8 520", 496.0, 0.0));
            rimlist.Add(new CRim("Sun ME14A 26\" MTB 559", 534.0, 0.0));
            rimlist.Add(new CRim("Sun ME14A 650C 571", 549.0, 0.0));
            rimlist.Add(new CRim("Sun ME14A 700C 622", 601.0, 0.0));
            rimlist.Add(new CRim("Sun Phat Albert 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno  20\" BMX 406", 392.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno  26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno  700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite (Damon measured Nov. 06 2002) 26\" MTB 559", 553.4, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite (MEAURE TO BE SURE!) 20\" BMX 406", 393.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite (MEAURE TO BE SURE!) 20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite (newer version and XL-1.75 version) 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite (newer version) 24\" BMX 507", 486.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite (newer version) 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite (XL version) 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite 20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite 24\" BMX 507", 493.0, 0.0));
            rimlist.Add(new CRim("Sun Rhyno Lite MEASURE TO BE SURE! 26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Sun SST20 20\" BMX 406", 392.0, 0.0));
            rimlist.Add(new CRim("Sun SST20 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("Sun SST20 700C 622", 603.0, 0.0));
            rimlist.Add(new CRim("Sun Sub IV 26\" MTB 559", 544.0, 0.0));
            rimlist.Add(new CRim("Sun Venus (medium deep aero) 650C 571", 540.0, 0.0));
            rimlist.Add(new CRim("Sun Venus (medium deep aero) 700C 622", 592.0, 0.0));
            rimlist.Add(new CRim("SUN太阳 M14A （此参数尚待核实） 20X1.5 406", 380.0, 0.0));
            rimlist.Add(new CRim("Super Champ Competition 27\" 630", 623.0, 0.0));
            rimlist.Add(new CRim("Synchros Lil' Snapper 26\" MTB 559", 542.0, 0.0));
            rimlist.Add(new CRim("Tioga XC 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Torelli Master 700C 622", 606.0, 0.0));
            rimlist.Add(new CRim("Torelli Master, new style 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Velocity Aero (20mm wide x 21mm deep) 700C 622", 596.0, 0.0));
            rimlist.Add(new CRim("Velocity Aero Heat AT (24mm wide x 22mm deep) 26\" MTB 559", 536.0, 0.0));
            rimlist.Add(new CRim("Velocity Aero Heat AT 20\" BMX 406", 383.0, 0.0));
            rimlist.Add(new CRim("Velocity Aero Heat AT 24\" BMX 507", 483.0, 0.0));
            rimlist.Add(new CRim("Velocity Aerohead (20mm wide x 19mm deep, 2001) 700C 622", 602.0, 0.0));
            rimlist.Add(new CRim("Velocity Aerohead (20mm wide x 19mm deep, 2001) MEASURE TO BE SURE! 650C 571", 558.0, 0.0));
            rimlist.Add(new CRim("Velocity Aerohead (20mm wide x 21mm deep) 20x1 1/8 451", 429.0, 0.0));
            rimlist.Add(new CRim("Velocity Aerohead (20mm wide x 21mm deep) 650C 571", 554.0, 0.0));
            rimlist.Add(new CRim("Velocity Aerohead (20mm wide x 21mm deep, 2000 and earlier) 700C 622", 598.0, 0.0));
            rimlist.Add(new CRim("Velocity Aerohead OC (Off Center) 700C 622", 598.0, 4.0));
            rimlist.Add(new CRim("Velocity Cliff Hanger (28mm wide x 25mm deep) 26\" MTB 559", 531.0, 0.0));
            rimlist.Add(new CRim("Velocity Deep V (19mm wide x 30mm deep) 650C 571", 531.0, 0.0));
            rimlist.Add(new CRim("Velocity Deep V (19mm wide x 30mm deep) 700C 622", 582.0, 0.0));
            rimlist.Add(new CRim("Velocity Deep V (24mm wide x 32mm deep) 20\" BMX 406", 364.0, 0.0));
            rimlist.Add(new CRim("Velocity Deep V (24mm wide x 32mm deep) 24\" BMX 507", 464.5, 0.0));
            rimlist.Add(new CRim("Velocity Deep V (24mm wide x 32mm deep) 26\" MTB 559", 517.0, 0.0));
            rimlist.Add(new CRim("Velocity Dyad (24mm wide x 22mm deep) 700C 622", 596.0, 0.0));
            rimlist.Add(new CRim("Velocity Escape 19mm x 19mm 650C 管胎专用", 554.0, 0.0));
            rimlist.Add(new CRim("Velocity Escape 19mm x 19mm 700C 管胎专用", 602.0, 0.0));
            rimlist.Add(new CRim("Velocity Fusion (19mm wide x 25mm deep) Measured 596.4. MEASURE TO BE SURE! 700C 622", 591.0, 0.0));
            rimlist.Add(new CRim("Velocity Glider (26mm wide x 14mm deep) 700C 622", 612.0, 0.0));
            rimlist.Add(new CRim("Velocity K525 (21mm wide x 21mm deep) 26\" MTB 559", 538.0, 0.0));
            rimlist.Add(new CRim("Velocity Mt. GoAT (25mm wide x 18mm deep) 26\" MTB 559", 542.0, 0.0));
            rimlist.Add(new CRim("Velocity Pro/Elite 650C 管胎专用", 531.0, 0.0));
            rimlist.Add(new CRim("Velocity Pro/Elite 700C 管胎专用", 582.0, 0.0));
            rimlist.Add(new CRim("Velocity Razor (20mm wide x 16mm deep) 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Velocity Razor (like Mavic Open Pro) 650C 571", 556.0, 0.0));
            rimlist.Add(new CRim("Velocity Synergy (23mm wide x 20mm deep, symmetrical and asymmetrical) 26\" MTB 559", 540.0, 0.0));
            rimlist.Add(new CRim("Velocity TAIPAN 20\" BMX 406", 380.0, 0.0));
            rimlist.Add(new CRim("Velocity TAIPAN 24\" BMX 507", 480.0, 0.0));
            rimlist.Add(new CRim("Velocity Triple V  20\" BMX 406", 395.0, 0.0));
            rimlist.Add(new CRim("Velocity Triple V  24\" BMX 507", 495.5, 0.0));
            rimlist.Add(new CRim("Velocity Triple V (26mm wide x 17mm deep) 26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Velocity Twin Hollow (22mm wide x 14mm deep) 700C 622", 611.0, 0.0));
            rimlist.Add(new CRim("Velocity Twin Hollow 27\" 630", 619.0, 0.0));
            rimlist.Add(new CRim("Veulta Airline 3  700C 622", 585.0, 0.0));
            rimlist.Add(new CRim("Vuelta Airline 1 20\" BMX 406", 376.9, 0.0));
            rimlist.Add(new CRim("Vuelta Airline 1 26\" MTB 559", 529.3, 0.0));
            rimlist.Add(new CRim("Vuelta Airline 2 622x13 700C 622", 595.9, 0.0));
            rimlist.Add(new CRim("Vuelta Excalibur DiscDH 559x27 26\" MTB 559", 540.9, 0.0));
            rimlist.Add(new CRim("Vuelta Excalibur DiscFR 559x21 26\" MTB 559", 543.6, 0.0));
            rimlist.Add(new CRim("Vuelta Excalibur DiscXC 559x21 26\" MTB 559", 543.5, 0.0));
            rimlist.Add(new CRim("Vuelta Strong, no eyelets 26\" MTB 559", 547.1, 0.0));
            rimlist.Add(new CRim("Vuelta Stylus 622x13 700C 622", 599.3, 0.0));
            rimlist.Add(new CRim("Vuelta Vision 559x18 26\" MTB 559", 543.2, 0.0));
            rimlist.Add(new CRim("Vuelta Vision 700C 622", 607.0, 0.0));
            rimlist.Add(new CRim("Weinmann 2115  27\" 630", 621.0, 0.0));
            rimlist.Add(new CRim("Weinmann 2120 26x1 3/8 26x1 3/8 590", 579.0, 0.0));
            rimlist.Add(new CRim("Weinmann 500A 440", 436.0, 0.0));
            rimlist.Add(new CRim("Weinmann A129  20\" BMX 406", 400.0, 0.0));
            rimlist.Add(new CRim("Weinmann A129  27\" 630", 624.0, 0.0));
            rimlist.Add(new CRim("Weinmann A129  700C 622", 619.0, 0.0));
            rimlist.Add(new CRim("Weinmann Mod 2719  26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("Weinmann Zac19  20\" BMX 406", 387.0, 0.0));
            rimlist.Add(new CRim("Wheelsmith Cowpie  26\" MTB 559", 551.0, 0.0));
            rimlist.Add(new CRim("Wheelsmith Rimfire  700C 622", 609.0, 0.0));
            rimlist.Add(new CRim("Wheelsmith Speedcow  26\" MTB 559", 546.0, 0.0));
            rimlist.Add(new CRim("Wolber Aspin  700C 管胎专用", 614.0, 0.0));
            rimlist.Add(new CRim("Wolber AT15 26\" MTB 559", 551.0, 0.0));
            rimlist.Add(new CRim("Wolber AT18 26\" MTB 559", 550.0, 0.0));
            rimlist.Add(new CRim("Wolber AT20  26\" MTB 559", 547.0, 0.0));
            rimlist.Add(new CRim("Wolber Canyon  26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("Wolber GTA 24x1 3/8 540", 530.0, 0.0));
            rimlist.Add(new CRim("Wolber M58  650B 584", 574.0, 0.0));
            rimlist.Add(new CRim("Wolber M58, GTA , GTX  27\" 630", 620.0, 0.0));
            rimlist.Add(new CRim("Wolber Profil 18  24\" 管胎专用", 511.0, 0.0));
            rimlist.Add(new CRim("Wolber Profil 18  650C 管胎专用", 558.0, 0.0));
            rimlist.Add(new CRim("Wolber Profil 18  700C 管胎专用", 611.0, 0.0));
            rimlist.Add(new CRim("Wolber Profil 20  650C 管胎专用", 550.0, 0.0));
            rimlist.Add(new CRim("Wolber Profil 20  700C 管胎专用", 600.0, 0.0));
            rimlist.Add(new CRim("Wolber Profil A  700C 管胎专用", 608.0, 0.0));
            rimlist.Add(new CRim("Wolber TX Profil aero 700C 622", 602.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Arc-en-ciel 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Aspin 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Aubisque 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Competition 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Gentleman 81 27\" 630", 620.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Gentleman 81 700C 622", 613.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Gentleman GTA2 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Gentleman GTA2 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Gentleman GTX 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Gentleman GTX 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Medaile d'Or 700C 管胎专用", 616.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Module 58 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Module 58 700C 622", 615.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Module 59 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion Module 59 700C 622", 615.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion T430 Alpine 27\" 630", 622.0, 0.0));
            rimlist.Add(new CRim("Wolber/Super Champion T430 Alpine 700C 622", 614.0, 0.0));
            rimlist.Add(new CRim("Wood, unknown make, vintage 1900 or so. 28\" 管胎专用", 622.3, 0.0));
            rimlist.Add(new CRim("WTB Dual Duty 30x11.8 (32h, 30mm wide) 26\" MTB 559", 536.0, 0.0));
            rimlist.Add(new CRim("WTB LaserBeam 22x11.8 (32h, 22mm wide) 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("WTB LaserBeam 25x11.3 (32h, 25mm wide) 26\" MTB 559", 538.0, 0.0));
            rimlist.Add(new CRim("WTB LaserDisc DH 36x0 (32h, 36mm wide) 26\" MTB 559", 540.0, 0.0));
            rimlist.Add(new CRim("WTB LaserDisc XC 24x0 (32h, 24mm wide) 26\" MTB 559", 542.0, 0.0));
            rimlist.Add(new CRim("WTB PowerBeam  26\" MTB 559", 548.0, 0.0));
            rimlist.Add(new CRim("WTB PowerBeam (old) 26\" MTB 559", 545.0, 0.0));
            rimlist.Add(new CRim("WTB SpeedDisc XC 24x0 (32h, 24mm wide) 26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("WTB SpeedMaster 21x11.3 700c (32h, 21mm wide)  700C 622", 606.0, 0.0));
            rimlist.Add(new CRim("WTB SpeedMaster 23x11.3 (32h, 23mm wide)  26\" MTB 559", 541.0, 0.0));
            rimlist.Add(new CRim("WTB SpeedMaster 25mm  26\" MTB 559", 552.0, 0.0));
            rimlist.Add(new CRim("WTB SpeedMaster II  26\" MTB 559", 549.0, 0.0));
            rimlist.Add(new CRim("Zipp 110 17mm deep box section aluminum clincher 700C 622", 605.0, 0.0));
            rimlist.Add(new CRim("Zipp 140 12mm deep box section aluminum tubular 700C 管胎专用", 614.0, 0.0));
            rimlist.Add(new CRim("Zipp 245 38mm deep carbon tubular 650C 管胎专用", 521.0, 0.0));
            rimlist.Add(new CRim("Zipp 250 24.8mm deep carbon tubular 700C 管胎专用", 596.0, 0.0));
            rimlist.Add(new CRim("Zipp 280 38mm deep carbon tubular  700C 管胎专用", 569.0, 0.0));
            rimlist.Add(new CRim("Zipp 310 30mm deep aluminum clincher  700C 622", 584.0, 0.0));
            rimlist.Add(new CRim("Zipp 310 38mm deep carbon tubular 650C 管胎专用", 520.0, 0.0));
            rimlist.Add(new CRim("Zipp 330 58mm deep carbon tubular 650C 管胎专用", 484.0, 0.0));
            rimlist.Add(new CRim("Zipp 340 38mm deep carbon tubular 700C 管胎专用", 566.0, 0.0));
            rimlist.Add(new CRim("Zipp 360 58mm deep carbon tubular 700C 管胎专用", 530.0, 0.0));
            rimlist.Add(new CRim("Zipp 370 40mm deep carbon/aluminum clincher 650C 571", 518.0, 0.0));
            rimlist.Add(new CRim("Zipp 400 58mm deep carbon tubular 650C 管胎专用", 482.0, 0.0));
            rimlist.Add(new CRim("Zipp 415 40mm deep carbon/aluminum clincher 700C 622", 567.0, 0.0));
            rimlist.Add(new CRim("Zipp 420 82mm deep carbon tubular 700C 管胎专用", 484.0, 0.0));
            rimlist.Add(new CRim("Zipp 440 58mm deep carbon tubular 700C 管胎专用", 530.0, 0.0));
            rimlist.Add(new CRim("Zipp 460 58mm deep carbon/aluminum clincher 650C 571", 477.0, 0.0));
            rimlist.Add(new CRim("Zipp 500 58mm deep carbon/aluminum clincher 650C 571", 482.0, 0.0));
            rimlist.Add(new CRim("Zipp 505 58mm deep carbon/aluminum clincher 700C 622", 529.0, 0.0));
            rimlist.Add(new CRim("Zipp 520 82mm deep carbon/aluminum clincher 700C 622", 477.0, 0.0));
            rimlist.Add(new CRim("Zipp 530 58mm deep carbon/aluminum clincher 700C 622", 528.0, 0.0));
            rimlist.Add(new CRim("大行SRA683原装WTB双层车圈 16x1.5 305", 286.0, 0.0));
            rimlist.Add(new CRim("见泰DWS009小刀圈(比较难扒胎） 20X1.5 406", 392.0, 0.0));

        }
    }
}
