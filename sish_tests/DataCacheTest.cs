﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using sish;
using System;

namespace sish_tests
{
    [TestClass]
    public class DataCacheTest
    {
        [TestMethod]
        public void TestGetFromFileSystem()
        {
            Console.WriteLine("Running in " + System.AppDomain.CurrentDomain.BaseDirectory);
            
            Assert.AreEqual("{\r\n\t\"code\": \"BLAH\"\r\n}", DataCache.getOrLoadJsonFile(null, "../../../BLAH.json"));
        }
    }
}
