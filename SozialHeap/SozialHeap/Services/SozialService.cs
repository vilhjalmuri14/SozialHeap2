using Sozialheap.Models;
using Sozialheap.Models.ViewModels;
using SozialHeap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sozialheap.Services
{
    public class SozialService
    {
        // The database (our one, not the authentication one)
        private SozialheapEntities db2 = new SozialheapEntities();

        /// <summary>
        /// Function returns list of all groups from the database
        /// </summary>
        /// <returns>List of all groups</returns>
        public List<Group> GetAllGroups()
        {
            var groups = (from item in db2.Groups
                          select item).ToList();
            return groups;
        }

        /// <summary>
        /// Returns the group with the provided id
        /// </summary>
        /// <param name="id">id of the desired group</param>
        /// <returns>single group</returns>
        public Group GetGroupById(int id)
        {
            var group = (from item in db2.Groups
                         where item.groupID == id
                         select item).Single<Group>();
            return (Group)group;
        }

        /// <summary>
        /// Inserts a new Group to the DB.
        /// </summary>
        /// <param name="g">Group object</param>
        public void CreateGroup(Group g)
        {
            // TODO: Implement creategroup !
        }

        /// <summary>
        /// Updates the group information by the given group by its id
        /// </summary>
        /// <param name="g">Group to update</param>
        public void EditGroup(Group g)
        {
            // TODO: Implement EDIT !
        }

        /// <summary>
        /// Deletes the group with the provided id ! Note that the DB might refuse if we have dependent Questions
        /// </summary>
        /// <param name="id"></param>
        public void DeleteGroup(int id)
        {
            // TODO: Implement deletegroup 
        }

        /// <summary>
        /// Returns a list of all posts by given id of Group.
        /// </summary>
        /// <param name="groupId">id of Group</param>
        /// <returns>List of all post of that group</returns>
        public List<Post> getPosts(int groupId)
        {
            List<Post> p = (from item in db2.Posts
                            where item.groupID == groupId
                            orderby item.dateCreated
                            select item).ToList();

            return p;
        }

        /// <summary>
        /// Returns post by given id.
        /// </summary>
        /// <param name="groupId">id of Post</param>
        /// <returns>one Post</returns>
        public Post getPost(int postId)
        {
            Post p = (from item in db2.Posts
                      where item.postID == postId
                      orderby item.dateCreated
                      select item).SingleOrDefault();

            return p;
        }

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="p">Post to insert</param>
        public void CreatePost(Post p)
        {
            // TODO: implement db insert
        }

        /// <summary>
        /// Delete post with the given id
        /// </summary>
        /// <param name="id">id of post to delete</param>
        public void DeletePost(int id)
        {
            // TODO: implement delete Post by id
        }

        /// <summary>
        /// Update the post to the provided one, id cannot be changed
        /// </summary>
        /// <param name="p">Post to update</param>
        public void EditPost(Post p)
        {

            // TODO implement edit Post
        }

        /// <summary>
        /// Returns single user by the provided id
        /// </summary>
        /// <param name="id">string object for nvarchar(128) in DB.</param>
        /// <returns>single User object</returns>
        public User GetUserById(string id)
        {
            var user = (from item in db2.Users
                        where (item.userID == id)
                        select item).Take(1);

            return (User)user;
        }

        /// <summary>
        /// Returns single user by username
        /// </summary>
        /// <param name="username">username of desired user</param>
        /// <returns>User object</returns>
        public User GetUserByUsername(string username)
        {
            var user = (from item in db2.Users
                            where item.userName == "username"
                            select item).SingleOrDefault();
            return user;
        }

        /// <summary>
        /// Returns a list of Users that starts with the given query string
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<User> GetUsersByQuery(string query)
        {
            var users = (from item in db2.Users
                         where item.userName.StartsWith(query)
                         select item).ToList();

            return users;
        }

        public List<User> GetUsersByGroup(int groupId, int n = 5)
        {
            // var users = db2.Database.

            return new List<User>();
        }


        /// <summary>
        /// Returns list of all the users in the database
        /// </summary>
        /// <returns>List of Users (all)</returns>
        public List<User> GetAllUsers()
        {
            var users = (from item in db2.Users
                         select item).ToList();
            
            List<User> nUsers = new List<User>();

            foreach (var i in users)
            {
                //nUsers.Add(new User(i.userID, i.userName));
            }

            return nUsers;
        }

        /// <summary>
        /// Returns the Answers with the provided id
        /// </summary>
        /// <param name="id">id of the desired post</param>
        /// <returns>list of answers</returns>
        public List<Answer> GetAnswerById(int id)
        {
            var answers = (from item in db2.Answers
                         where item.postID == id
                         select item).ToList();
            return (List<Answer>)answers;
        }
    }
}