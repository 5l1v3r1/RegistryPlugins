﻿using System;
using System.ComponentModel;
using System.Linq;
using NFluent;
using NUnit.Framework;
using Registry;
using RegistryExplorer.MountedDevices;
using RegistryPlugin.AppCompatCache;
using RegistryPlugin.CIDSizeMRU;
using RegistryPlugin.FirstFolder;
using RegistryPlugin.KnownNetworks;
using RegistryPlugin.LastVisitedMRU;
using RegistryPlugin.LastVisitedPidlMRU;
using RegistryPlugin.OpenSaveMRU;
using RegistryPlugin.OpenSavePidlMRU;
using RegistryPlugin.RecentDocs;
using RegistryPlugin.RunMRU;
using RegistryPlugin.SAM;
using RegistryPlugin.WordWheelQuery;
using ValuesOut = RegistryPlugin.AppCompatCache.ValuesOut;

namespace RegistryPlugins.Test
{
    [TestFixture]
    public class PluginTests
    {
        [Test]
        public void AppCompatTest()
        {
            var r = new AppCompat();

            var reg = new RegistryHive(@"D:\Sync\RegistryHives\SYSTEM");
            reg.ParseHive();

            var key = reg.GetKey(@"ControlSet001\Control\Session Manager\AppCompatCache");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(1024);

            var ff = (ValuesOut) r.Values[0];

            Check.That(ff.CacheEntryPosition).IsEqualTo(0);
            Check.That(ff.ProgramName).Contains("java");
        }

        [Test]
        public void BlakeRecentDocs()
        {
            var r = new RecentDocs();

            var reg = new RegistryHive(@"D:\Sync\RegistryHives\NTUSER_dblake.DAT");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\RecentDocs");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(192);
            Check.That(r.Errors.Count).IsEqualTo(0);

            var ff = (RecentDoc)r.Values[0];

            Check.That(ff.ValueName).IsEqualTo("83");
            Check.That(ff.Extension).Contains("RecentDocs");
        }

        [Test]
        public void BlakeWordWheel()
        {
            var r = new WordWheelQuery();

            var reg = new RegistryHive(@"D:\Sync\RegistryHives\NTUSER_dblake.DAT");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\WordWheelQuery");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(3);
            Check.That(r.Errors.Count).IsEqualTo(0);

            var ff = (RegistryPlugin.WordWheelQuery.ValuesOut)r.Values[0];

            Check.That(ff.MruPosition).IsEqualTo(0);
            Check.That(ff.SearchTerm).Contains("defrag");

            ff = (RegistryPlugin.WordWheelQuery.ValuesOut)r.Values[1];

            Check.That(ff.MruPosition).IsEqualTo(1);
            Check.That(ff.SearchTerm).Contains("jboone");

            ff = (RegistryPlugin.WordWheelQuery.ValuesOut)r.Values[2];

            Check.That(ff.MruPosition).IsEqualTo(2);
            Check.That(ff.SearchTerm).Contains("valhalla");
        }

        [Test]
        public void BlakeOpenSavePidlMRU()
        {
            var r = new OpenSavePidlMRU();

            var reg = new RegistryHive(@"D:\Sync\RegistryHives\NTUSER_dblake.DAT");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\OpenSavePidlMRU");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(57);
            Check.That(r.Errors.Count).IsEqualTo(0);

            var ff = (RegistryPlugin.OpenSavePidlMRU.ValuesOut)r.Values[0];

            Check.That(ff.AbsolutePath).IsEqualTo(@"Web sites\https://asgardventurecapital.sharepoint.com\Shared Documents\Confidential Analysis Data\NETFLIX_10-K_20130201.xlsx");
            Check.That(ff.ValueName).IsEqualTo("17");
        }


        [Test]
        public void BlakeKnownNetworks()
        {
            var r = new KnownNetworks();

            var reg = new RegistryHive(@"D:\Sync\RegistryHives\SOFTWARE_dblake");
            reg.ParseHive();

            var key = reg.GetKey(@"Microsoft\Windows NT\CurrentVersion\NetworkList");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(27);
            Check.That(r.Errors.Count).IsEqualTo(0);

            var ff = (KnownNetwork)r.Values[0];

            Check.That(ff.NetworkName).IsEqualTo(@"gogoinflight");
            Check.That(ff.DNSSuffix).IsEqualTo(@"<none>");
            Check.That(ff.ProfileGUID).IsEqualTo("{167B2E5E-29EA-429E-8D43-E82043F0D3CF}");
            Check.That(ff.FirstConnect.Year).IsEqualTo(2013);
            Check.That(ff.FirstConnect.Day).IsEqualTo(3);


        }

        [Test]
        public void CidSizeTest()
        {
            var r = new CIDSizeMRU();

            var reg = new RegistryHive(@"..\..\Hives\ntuser1.dat");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\CIDSizeMRU");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(8);

            var ff = (CIDSizeInfo) r.Values[0];

            Check.That(ff.MRUPosition).IsEqualTo(7);
            Check.That(ff.Executable).Contains("CCleaner");
        }


        //        [Test]
        //        public void LastVisitedPidlMruTest2()
        //        {
        //            var r = new LastVisitedPidlMRU();
        //            var reg = new RegistryHive(@"D:\Temp\win10ERZamcachepreso\NTUSER.DAT");
        //            reg.ParseHive();
        //
        //            var key = reg.GetKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\LastVisitedPidlMRU");
        //
        //            Check.That(r.Values.Count).IsEqualTo(0);
        //
        //            r.ProcessValues(key);
        //
        //            Check.That(r.Values.Count).IsEqualTo(6);
        //
        //        }


        [Test]
        public void FirstFolderTest()
        {
            var r = new FirstFolder();

            var reg = new RegistryHive(@"..\..\Hives\ntuser1.dat");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\FirstFolder");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(3);

            var ff = (FolderInfo) r.Values[0];

            Check.That(ff.MRUPosition).IsEqualTo(2);
            Check.That(ff.Executable).Contains("CCleaner");
        }

        [Test]
        public void LastVisitedMRUTest()
        {
            var r = new LastVisitedMRU();
            var reg = new RegistryHive(@"d:\Sync\RegistryHives\ntuserNokiaShellBags.dat");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\LastVisitedMRU");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(11);
        }

        [Test]
        public void LastVisitedPidlMruTest()
        {
            var r = new LastVisitedPidlMRU();
            var reg = new RegistryHive(@"..\..\Hives\ntuser1.dat");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\LastVisitedPidlMRU");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(7);
        }

        [Test]
        public void MountedDevicesTest()
        {
            var r = new MountedDevices();

            var reg = new RegistryHive(@"D:\Sync\RegistryHives\SYSTEM");
            reg.ParseHive();

            var key = reg.GetKey(@"MountedDevices");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(65);
        }

        [Test]
        public void OpenSaveMRUTest()
        {
            var r = new OpenSaveMRU();
            var reg = new RegistryHive(@"D:\Sync\RegistryHives\NTUSER_Loveall.DAT");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\OpenSaveMRU");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(4);
        }

        [Test]
        public void OpenSavePidlMruTest()
        {
            var r = new OpenSavePidlMRU();
            var reg = new RegistryHive(@"..\..\Hives\ntuser1.dat");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\OpenSavePidlMRU");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(16);
        }

        [Test]
        public void RunMRUTest()
        {
            var r = new RunMRU();
            var reg = new RegistryHive(@"d:\Sync\RegistryHives\ALL\NTUSER (8).DAT");
            reg.ParseHive();

            var key = reg.GetKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsEqualTo(4);
        }

        [Test]
        public void SamPluginShouldFindEricAccount()
        {
            var r = new UserAccounts();

            var reg = new RegistryHive(@"..\..\Hives\SAM");
            reg.ParseHive();

            var key = reg.GetKey(@"SAM\Domains\Account\Users");

            Check.That(r.Values.Count).IsEqualTo(0);

            r.ProcessValues(key);

            Check.That(r.Values.Count).IsGreaterThan(0);
            Check.That(r.Values.Count).IsEqualTo(4);

            var users = (BindingList<UserOut>) r.Values;

            var user = users.Single(t => t.UserId == 1000);

            Check.That(user.UserName.ToLowerInvariant()).IsEqualTo("eric");

            var lastLogin = DateTimeOffset.Parse("3/25/2015 2:31:24 PM +00:00");

            Check.That(user.LastLoginTime?.ToString("G")).Equals(lastLogin.ToString("G"));

            Check.That(user.LastLoginTime?.Year).Equals(lastLogin.Year);
            Check.That(user.LastLoginTime?.Month).Equals(lastLogin.Month);
            Check.That(user.LastLoginTime?.Day).Equals(lastLogin.Day);
            Check.That(user.LastLoginTime?.Hour).Equals(lastLogin.Hour);
            Check.That(user.LastLoginTime?.Minute).Equals(lastLogin.Minute);
            Check.That(user.LastLoginTime?.Second).Equals(lastLogin.Second);

            var lastPwChange = DateTimeOffset.Parse("3/23/2015 9:15:55 PM +00:00");

            Check.That(user.LastPasswordChange?.ToString("G")).Equals(lastPwChange.ToString("G"));

            Check.That(user.LastIncorrectPassword).IsNull();
            Check.That(user.ExpiresOn).IsNull();

            Check.That(user.TotalLoginCount).IsEqualTo(3);
            Check.That(user.InvalidLoginCount).IsEqualTo(0);

            user = users.Single(t => t.UserId == 500);

            Check.That(user.UserName.ToLowerInvariant()).IsEqualTo("administrator");
        }
    }
}