using Sozialheap.Models;
using Sozialheap.Models.ViewModels;
using SozialHeap.Models;
using SozialHeap.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                         select item).SingleOrDefault<Group>();
            return (Group)group;
        }

        /// <summary>
        /// Inserts a new Group to the DB.
        /// </summary>
        /// <param name="g">Group object</param>
        public void CreateGroup(Group g)
        {
            db2.Groups.Add(g);
            db2.SaveChanges();
            // TODO: Implement creategroup !
        }

        public void EditGroup(Group g)
        {
            // testing update!
            Group newGroup = GetGroupById(g.groupID);
            newGroup = g;
            db2.SaveChanges();
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
            List<Post> posts = (from item in db2.Posts
                            where item.groupID == groupId
                            orderby item.dateCreated descending
                            select item).ToList();

            return posts;
        }
        
        public List<Post> getRecentPosts()
        {
            return (from item in db2.Posts orderby item.dateCreated descending select item).Take(5).ToList();
        }

        /// <summary>
        /// Returns posts by given userID.
        /// </summary>
        /// <returns>list of Posts</returns>
        public List<Post> getPostbyId(string id)
        {
            List<Post> p = (from item in db2.Posts
                            where item.userID == id
                            orderby item.dateCreated descending
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
                      orderby item.dateCreated descending
                      select item).SingleOrDefault();

            return p;
        }

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="p">Post to insert</param>
        public void CreatePost(Post p)
        {
            // add the attached Post

            db2.Posts.Add(p);
            db2.SaveChanges();
        }

        public void CreateAnswer(Answer a)
        {
            db2.Answers.Add(a);
            db2.SaveChanges();
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
                            where item.userName == username
                            select item).SingleOrDefault();
            return user;
        }

        /// <summary>
        /// Returns a list of Users that starts with the given query string
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<SozialHeap.Models.ViewModels.SimpleUser> GetUsersByQuery(string query)
        {
            List<User> users = (from item in db2.Users
                         where item.userName.StartsWith(query)
                         select item).ToList();
            List<SozialHeap.Models.ViewModels.SimpleUser> su = new List<SozialHeap.Models.ViewModels.SimpleUser>();

            foreach(var item in users)
            {
                su.Add(new SimpleUser(item.userName, item.userID));
            }
            return su;
        }

        public List<User> GetUsersByGroup(int groupId, int n = 5)
        {
            // TODO IMPLEMENT !
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
            
            return users;
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
                         orderby item.dateCreated descending
                         select item).ToList();
            return (List<Answer>)answers;
        }


        public void LikePost(int postID, string username)
        {
            var sqlParams = new List<SqlParameter>
             {
              new SqlParameter("@postID", postID.ToString()),
              new SqlParameter("@userName", username)
             };
            db2.Database.ExecuteSqlCommand("exec pLikePost 2, 'lommi'", new SqlParameter());
        }
    }
}