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

            // adding 3 groups
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
            
            // adding 2 posts
            Post p = new Post
            {
                postID = 1,
                groupID = 2,
                userID = "1234",
                name = "C++ problem",
                body = "how to make not in c++?"
            };
            MockDb.Posts.Add(p);

            Post p1 = new Post
            {
                postID = 2,
                groupID = 2,
                userID = "1234",
                scoreCounter = 0,
                name = "C++ not working!!",
                body = "Help my compiler is broken."
            };
            MockDb.Posts.Add(p1);

            // Adding 3 users
            User u = new User
            {
                userID = "1234",
                userName = "John",
                score = 0,
                description = "full time farmer."
            };
            MockDb.Users.Add(u);

            User u1 = new User
            {
                userID = "4321",
                userName = "Tomas",
                description = "I love programming."
            };
            MockDb.Users.Add(u1);

            User u2 = new User
            {
                userID = "6789",
                userName = "Tom",
                description = "The programmer."
            };
            MockDb.Users.Add(u2);

            // adding 2 answers
            Answer a = new Answer
            {
                answerID = 1,
                postID = 2,
                title = "Try this one.",
                body = "Have you tryed the GCC compiler."
            };
            MockDb.Answers.Add(a);

            Answer a1 = new Answer
            {
                answerID = 2,
                postID = 2,
                title = "Try this.",
                body = "Have you tryed using a hammer."
            };
            MockDb.Answers.Add(a1);


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
            Group g = new Group
            {
                groupID = 3,
                groupName = "javascript",
                description = "Programming"
            };
            service.EditGroup(g);
            var result = service.GetGroupById(3);

            // Assert:
            Assert.AreEqual("javascript", result.groupName);
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

        [TestMethod]
        public void TestGetPostByUserId()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.getPostbyUserId("1234");
         
            // Assert:
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void TestGetUserByUsernameEmma()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.GetUserByUsername("Tom");

            // Assert:
            Assert.AreEqual("6789", result.userID);
        }

        [TestMethod]
        public void TestGetUserByUsernameJohn()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.GetUserByUsername("John");

            // Assert:
            Assert.AreEqual("1234", result.userID);
        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.GetAllUsers();

            // Assert:
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void TestGetAnswerById()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.GetAnswerById(2);

            // Assert:
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void TestGetAnswerById1()
        {
            // Arrenge:
            var service = _service;

            // Act:
            var result = service.GetAnswerById(1);

            // Assert:
            Assert.AreEqual(0, result.Count);
        }
    }
}
