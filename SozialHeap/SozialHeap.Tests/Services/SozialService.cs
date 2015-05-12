using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sozialheap.Test;
using SozialHeap.Models;

namespace SozialHeap.Tests.Services
{
    [TestClass]
    public class SozialService
    {
        private Sozialheap.Services.SozialService _service;

        [TestInitialize]
        public void Initialize()
        {
            var MockDb = new MockDatabase();

            Group g1 = new Group
            {
                groupID = 1,
                groupName = "c++",
                description = "Programming"
            };
            MockDb.Groups.Add(g1);

            Group g2 = new Group
            {
                groupID = 2,
                groupName = "c++",
                description = "Programming"
            };
            MockDb.Groups.Add(g2);

            Group g3 = new Group
            {
                groupID = 3,
                groupName = "c++",
                description = "Programming"
            };
            MockDb.Groups.Add(g3);

            _service = new Sozialheap.Services.SozialService(MockDb);
        }

        [TestMethod]
        public void TestGetAllGroups()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.GetAllGroups();

            // Assert:
            Assert.AreEqual(3, result.Count);
        }
    }
}
