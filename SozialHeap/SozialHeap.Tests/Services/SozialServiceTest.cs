using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sozialheap.Test;
using SozialHeap.Models;

namespace SozialHeap.Tests.Services
{
    [TestClass]
    public class SozialServiceTest
    {
        private Sozialheap.Services.SozialService _service;

        [TestInitialize]
        public void Initialize()
        {
            var MockDb = new MockDatabase();

            Group g1 = new Group
            {
                groupID = 1,
                groupName = "php",
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
                groupName = "java",
                description = "Programming"
            };
            MockDb.Groups.Add(g3);
            /*
            Post p = new Post
            {
                postID = 1,
                groupID = 2,
                name = "C++ problem",
                body = "how to make not in c++?"
            };
            MockDb.Posts.Add(p); */

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

        [TestMethod]
        public void TestGetGroupById2()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.GetGroupById(2);

            // Assert:
            Assert.AreEqual("c++", result.groupName);
        }

        [TestMethod]
        public void TestGetGroupById3()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.GetGroupById(3);

            // Assert:
            Assert.AreEqual("java", result.groupName);
        }

        [TestMethod]
        public void TestCreateGroup()
        {
            // Arrenge:
            var service = _service;

            var g = new Group{
                groupName = "python",
                groupID = 4,
                description = "programming whith me."
            };

            // Act:
            service.CreateGroup(g);
            var result = service.GetAllGroups();

            // Assert:
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public void TestCreateGroup1()
        {
            // Arrenge:
            var service = _service;

            var g = new Group
            {
                groupName = "assembly",
                groupID = 5,
                description = "very hard."
            };

            // Act:
            service.CreateGroup(g);
            var result = service.GetGroupById(5);

            // Assert:
            Assert.AreEqual("very hard.", result.description);
        }

        [TestMethod]
        public void TestEditGroup()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var group = service.GetGroupById(2);
            group.description = "professional.";
            var result = service.GetGroupById(2);

            // Assert:
            Assert.AreEqual("professional.", group.description);
        }

        [TestMethod]
        public void TestGetPost()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.getPost(1);

            // Assert:
            Assert.AreEqual(2, result.groupID);
        }
    }
}
