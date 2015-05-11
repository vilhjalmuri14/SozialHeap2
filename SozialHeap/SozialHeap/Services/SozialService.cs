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
        private bool checkNetwork()
        {
            return true;
        }
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
            try
            {
                db2.Groups.Add(g);
                db2.SaveChanges();
            }
            catch(Exception ex)
            {
                
                return;
            }
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
            User user = (from item in db2.Users
                        where (item.userID == id)
                        select item).SingleOrDefault();

            return user;
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
                         orderby item.score descending
                         select item).ToList();
            List<SozialHeap.Models.ViewModels.SimpleUser> su = new List<SozialHeap.Models.ViewModels.SimpleUser>();

            foreach(var item in users)
            {
                su.Add(new SimpleUser(item.userName, item.userName));
            }
            return su;
        }

        public List<User> GetUsersByGroup(Group grp, int n = 5)
        {
            // TODO IMPLEMENT !
            List<User> users = (from item in db2.Users
                                where item.Groups.Contains(grp)
                                orderby item.score descending
                                select item).ToList();
            
            return new List<User>();
        }


        /// <summary>
        /// Returns list of all the users in the database
        /// </summary>
        /// <returns>List of Users (all)</returns>
        public List<User> GetAllUsers()
        {
            var users = (from item in db2.Users
                         orderby item.score descending
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

        /// <summary>
        /// Connect to another user (follow)
        /// </summary>
        /// <param name="currentUser">The user that wants to follow the next user</param>
        /// <param name="userToFollow">Ther user that he wants to follow</param>
        public void StartFollowingUser(User currentUser, User userToFollow)
        {
            // Users1 are those who are following the current user
            // Users are those who the user is following
            userToFollow.Users1.Add(currentUser);
            db2.SaveChanges();
        }

        /// <summary>
        /// User 1 stop following user 2
        /// </summary>
        /// <param name="currentUser">User who wants to stop following someone</param>
        /// <param name="userToStopFollow">The user who he wants to stop follow</param>
        public void StopFollowingUser(User currentUser, User userToStopFollow)
        {
            userToStopFollow.Users1.Remove(currentUser);
            db2.SaveChanges();

        }

        /// <summary>
        /// Check if user 1 is following user 2
        /// </summary>
        /// <param name="currentUser">Is this user following the next</param>
        /// <param name="isFollowing">Is this user being followed by the prior user</param>
        /// <returns></returns>
        public bool isFollowingUser(User currentUser, User isFollowing)
        {
            return isFollowing.Users1.Contains(currentUser);
        }

        /// <summary>
        /// User 1 wants to follow user 2
        /// </summary>
        /// <param name="user">User who wants to follow the next user</param>
        /// <param name="group">The user who he wants to follow</param>
        public void StartFollowingGroup(User user, Group group)
        {
            group.Users.Add(user);
            db2.SaveChanges();
        }

        /// <summary>
        /// User 1 wants to stop following user 2
        /// </summary>
        /// <param name="user">The user who wants to stop following user 2</param>
        /// <param name="group">The user he wants to stop following</param>
        public void StopFollowingGroup(User user, Group group)
        {
            group.Users.Remove(user);
            db2.SaveChanges();
        }

        /// <summary>
        /// Checks if a user is following a group
        /// </summary>
        /// <param name="user">User you want to check if is following the specified group</param>
        /// <param name="group">The group you want to check if he follows</param>
        /// <returns>true if a user is following the specified group</returns>
        public bool isFollowingGroup(User user, Group group)
        {
            return group.Users.Contains(user);
        }

        /// <summary>
        /// User puts a like on post
        /// </summary>
        /// <param name="user">User who wants to like a post</param>
        /// <param name="post">The post he wants to like</param>
        public void LikePost(User user, Post post)
        {
            user.Posts1.Add(post);
            post.scoreCounter++;
            if(post.User != user)
            {
                post.User.score += 5;
            }
            db2.SaveChanges();
        }

        /// <summary>
        /// User wants to unlike post
        /// </summary>
        /// <param name="user">User who wants to unlike a post</param>
        /// <param name="post">Post that the user wants to unlike</param>
        public void UnLikePost(User user, Post post)
        {
            user.Posts1.Remove(post);
            post.scoreCounter--;
            if (post.User != user)
            {
                post.User.score -= 5;
            }
            db2.SaveChanges();
        }

        /// <summary>
        /// Checks if a user has liked a post
        /// </summary>
        /// <param name="user">User you want to check if has liked the specified post</param>
        /// <param name="post">The post you want to check if he liked</param>
        /// <returns>true if a user has liked the specified group</returns>
        public bool DidUserLikePost(User user, Post post)
        {
            return user.Posts1.Contains(post);
        }

        /// <summary>
        /// Returns the Posts that have unseen answers by the owner(user)
        /// </summary>
        /// <param name="user">Owner of Question</param>
        /// <returns>List of Posts with unseen answers</returns>
        public List<Post> getUnreadPostsByUser(User user)
        {
            if (user != null)
            {
                return (List<Post>)(from item in db2.Answers
                                    where item.Post.userID == user.userID && item.seenByOwner == false
                                    select item.Post).ToList();
            }
            return new List<Post>();
        }
    }
}