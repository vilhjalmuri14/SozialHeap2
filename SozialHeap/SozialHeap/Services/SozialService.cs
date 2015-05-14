using Sozialheap.Models;
using Sozialheap.Models.ViewModels;
using SozialHeap.Models;
using SozialHeap.Models.ViewModels;
using SozialHeap.Utils;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sozialheap.Services
{
    public class SozialService
    {
        private readonly IAppDataContext db2;
        public SozialService(IAppDataContext context)
        {
            db2 = context ?? new SozialheapEntities();
        }

        /// <summary>
        /// Function returns list of all groups from the database
        /// </summary>
        /// <returns>List of all groups</returns>
        public List<Group> GetAllGroups()
        {
            try
            {
                var groups = (from item in db2.Groups
                              select item).ToList();
                return groups;

            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            return new List<Group>();
        }

        /// <summary>
        /// Returns the group with the provided id
        /// </summary>
        /// <param name="id">id of the desired group</param>
        /// <returns>single group</returns>
        public Group GetGroupById(int id)
        {
            try
            {
                var group = (from item in db2.Groups
                             where item.groupID == id
                             select item).SingleOrDefault<Group>();
                return (Group)group;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            return new Group();
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
                Utils.LogError(ex.Message.ToString());
                return;
            }
        }

        /// <summary>
        /// Edit the given group, only changes photo and description!
        /// </summary>
        /// <param name="g">group with changes</param>
        public void EditGroup(Group g)
        {
            // testing update!
            try
            {
                Group actualGroup = GetGroupById(g.groupID);
                actualGroup.photo = g.photo;
                actualGroup.description = g.description;

                db2.SaveChanges();
                return;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
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
            try 
            { 
                List<Post> posts = (from item in db2.Posts
                                where item.groupID == groupId
                                orderby item.dateCreated descending
                                select item).ToList();

                return posts;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            return new List<Post>();
        }
        
        /// <summary>
        /// Get the newest 5 posts in the system
        /// </summary>
        /// <returns>List of 5 newest posts List<Post></returns>
        public List<Post> getRecentPosts()
        {
            try 
            { 
                return (from item in db2.Posts orderby item.dateCreated descending select item).Take(5).ToList();
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

            return new List<Post>();
        }

        /// <summary>
        /// Returns post by given id.
        /// </summary>
        /// <param name="groupId">id of Post</param>
        /// <returns>one Post</returns>
        public Post getPost(int postId)
        {
            try
            {
                Post p = (from item in db2.Posts
                                    where item.postID == postId
                                    orderby item.dateCreated descending
                                    select item).SingleOrDefault();

                return p;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

            return new Post();
        }

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="p">Post to insert</param>
        public void CreatePost(Post p)
        {
            // add the attached Post
            try
            {
                db2.Posts.Add(p);
                db2.SaveChanges();
                return;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Create/Insert Answer
        /// </summary>
        /// <param name="a">Answer to insert</param>
        public void CreateAnswer(Answer a)
        {
            try
            {
                db2.Answers.Add(a);
                db2.SaveChanges();
                return;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Update the given answer
        /// </summary>
        /// <param name="a">Answer to update</param>
        public void EditAnswer(Answer a)
        {
            try
            {
                Answer edit = GetSingleAnswerByAnswerId(a.answerID);
                edit = a;
                db2.SaveChanges();
                return;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Delete given post
        /// </summary>
        /// <param name="postToDelete">Post to delete</param>
        public void DeletePost(Post postToDelete)
        {
            try 
            { 
                db2.Posts.Remove(postToDelete);
                db2.SaveChanges();
                return;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        
        }

        /// <summary>
        /// Delete Answer
        /// </summary>
        /// <param name="ansToDelete">Answer that should be removed</param>
        public void DeleteAnswer(Answer ansToDelete)
        {
            try
            {
                db2.Answers.Remove(ansToDelete);
                db2.SaveChanges();
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Update the post to the provided one, id cannot be changed
        /// </summary>
        /// <param name="p">Post to update</param>
        public void EditPost(Post p)
        {
            try
            {
                Post edit = getPost(p.postID);
                edit = p;
                db2.SaveChanges();
                return;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Update the User to the provided one.
        /// </summary>
        /// <param name="u">User to update</param>
        public void EditUser(User u)
        {
            try
            {
                User edit = GetUserById(u.userID);
                edit.fullName = u.fullName;
                edit.description = u.description;
                edit.photo = u.photo;

                db2.SaveChanges();
                return;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Returns single user by the provided id
        /// </summary>
        /// <param name="id">string object for nvarchar(128) in DB.</param>
        /// <returns>single User object</returns>
        public User GetUserById(string id)
        {
            try
            {
                User user = (from item in db2.Users
                             where (item.userID == id)
                             select item).SingleOrDefault();

                return user;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            return new User();
        }

        /// <summary>
        /// Returns posts by given userID.
        /// </summary>
        /// <returns>list of Posts</returns>
        public List<Post> getPostbyUserId(string id)
        {
            try
            {
                List<Post> p = (from item in db2.Posts
                                where item.userID == id
                                orderby item.dateCreated descending
                                select item).Take(5).ToList();

                return p;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            return null;
        }

        /// <summary>
        /// Returns single user by username
        /// </summary>
        /// <param name="username">username of desired user</param>
        /// <returns>User object</returns>
        public User GetUserByUsername(string username)
        {
            try
            {
            var user = (from item in db2.Users
                            where item.userName == username
                            select item).SingleOrDefault();
            return user;

            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

            return new User();
        }

        /// <summary>
        /// Get List of users by group
        /// </summary>
        /// <param name="grp">group</param>
        /// <param name="n">max number of users to return</param>
        /// <returns></returns>
        public List<User> GetUsersByGroup(Group grp, int n = 5)
        {
            try
            {
                List<User> users = (from item in db2.Users
                                    where item.Groups.Contains(grp)
                                    orderby item.score descending
                                    select item).Take(n).ToList();

                return new List<User>();
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

            return new List<User>();
        }


        /// <summary>
        /// Returns list of all the users in the database
        /// </summary>
        /// <returns>List of Users (all)</returns>
        public List<User> GetAllUsers()
        {
            try
            {
                var users = (from item in db2.Users
                             orderby item.score descending
                             select item).ToList();

                return users;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            
            return new List<User>();

        }

        /// <summary>
        /// Returns the Answers with the provided id
        /// </summary>
        /// <param name="id">id of the desired post</param>
        /// <returns>list of answers</returns>
        public List<Answer> GetAnswerById(int id)
        {
            try
            {
                var answers = (from item in db2.Answers
                             where item.postID == id
                             orderby item.dateCreated descending
                             select item).ToList();
                return (List<Answer>)answers;

            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

            return new List<Answer>();
        }

        /// <summary>
        /// Get single answer (for edit answer)
        /// </summary>
        /// <param name="answerID">id of the disired answer</param>
        /// <returns>Answer</returns>
        public Answer GetSingleAnswerByAnswerId(int answerID)
        {
            try
            {
                Answer res = (from item in db2.Answers
                              where item.answerID == answerID
                              select item).SingleOrDefault();
                return res;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            return null;
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
            try
            {
                userToFollow.Users1.Add(currentUser);
                db2.SaveChanges();
                return;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

        }

        /// <summary>
        /// User 1 stop following user 2
        /// </summary>
        /// <param name="currentUser">User who wants to stop following someone</param>
        /// <param name="userToStopFollow">The user who he wants to stop follow</param>
        public void StopFollowingUser(User currentUser, User userToStopFollow)
        {
            try
            {
                userToStopFollow.Users1.Remove(currentUser);
                db2.SaveChanges();
                return;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Check if user 1 is following user 2
        /// </summary>
        /// <param name="currentUser">Is this user following the next</param>
        /// <param name="isFollowing">Is this user being followed by the prior user</param>
        /// <returns></returns>
        public bool isFollowingUser(User currentUser, User isFollowing)
        {
            try
            {
                return isFollowing.Users1.Contains(currentUser);

            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

            return false;
        }

        /// <summary>
        /// User 1 wants to follow user 2
        /// </summary>
        /// <param name="user">User who wants to follow the next user</param>
        /// <param name="group">The user who he wants to follow</param>
        public void StartFollowingGroup(User user, Group group)
        {
            try
            {
                group.Users.Add(user);
                db2.SaveChanges();
                return;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// User 1 wants to stop following user 2
        /// </summary>
        /// <param name="user">The user who wants to stop following user 2</param>
        /// <param name="group">The user he wants to stop following</param>
        public void StopFollowingGroup(User user, Group group)
        {
            try
            {
                group.Users.Remove(user);
                db2.SaveChanges();
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Checks if a user is following a group
        /// </summary>
        /// <param name="user">User you want to check if is following the specified group</param>
        /// <param name="group">The group you want to check if he follows</param>
        /// <returns>true if a user is following the specified group</returns>
        public bool isFollowingGroup(User user, Group group)
        {
            try
            {
                return group.Users.Contains(user);
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

            return false;
        }

        /// <summary>
        /// User puts a like on post
        /// </summary>
        /// <param name="user">User who wants to like a post</param>
        /// <param name="post">The post he wants to like</param>
        public void LikePost(User user, Post post)
        {
            try
            {
                user.Posts1.Add(post);
                post.scoreCounter++;
                if (post.User != user)
                {
                    post.User.score += 5;
                }
                db2.SaveChanges();
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// User wants to unlike post
        /// </summary>
        /// <param name="user">User who wants to unlike a post</param>
        /// <param name="post">Post that the user wants to unlike</param>
        public void UnLikePost(User user, Post post)
        {
            try
            {
                user.Posts1.Remove(post);
                post.scoreCounter--;
                if (post.User != user)
                {
                    post.User.score -= 5;
                }
                db2.SaveChanges();
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Checks if a user has liked a post
        /// </summary>
        /// <param name="user">User you want to check if has liked the specified post</param>
        /// <param name="post">The post you want to check if he liked</param>
        /// <returns>true if a user has liked the specified group</returns>
        public bool DidUserLikePost(User user, Post post)
        {
            try
            {
                return user.Posts1.Contains(post);

            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            return false;
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
                // user is provided
                try
                {
                    List<Post> all = (List<Post>)(from item in db2.Answers
                                                  where item.Post.userID == user.userID && item.seenByOwner == false
                                                  select item.Post).ToList();
                    List<Post> res = new List<Post>();
                    foreach(var item in all)
                    {
                        // iterate to remove duplicates (we want only one item per Post)!
                        if(!res.Contains(item))
                        {
                            res.Add(item);
                        }
                    }
                    return res;
                }
                catch(Exception ex)
                {
                    Utils.LogError(ex.Message.ToString());
                }
            }
            return new List<Post>();
        }

        /// <summary>
        /// Acknowledge the unseen posts
        /// </summary>
        /// <param name="post">Post to mark owner seen</param>
        public void AcknowledgeNotifications(Post post)
        {
            try
            {
                foreach (var item in post.Answers)
                {
                    item.seenByOwner = true;
                }
                db2.SaveChanges();
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Find all Posts that contain the query string
        /// </summary>
        /// <param name="query">string with keyword(s)</param>
        /// <returns>List of Posts (results)</returns>
        public List<Post> findPostByString(string query)
        {
            List<Post> posts = (from item in db2.Posts
                                  where item.body.Contains(query) || item.name.Contains(query)
                                  select item).ToList();

            List<Answer> answers = (from item in db2.Answers
                                  where item.body.Contains(query) || item.title.Contains(query)
                                  select item).ToList();

            List<Post> final = new List<Post>();
            final = posts;
            
            for (int i = 0; i < answers.Count; i++ )
            {
                // Iterate to insert each post only once! (no duplicates)
                if(!final.Contains(answers[i].Post))
                {
                    final.Add(answers[i].Post);
                }
            }

            return final;
        }

        /// <summary>
        /// Find users by given querystring
        /// </summary>
        /// <param name="query">query string</param>
        /// <returns>List of users containing the query string</returns>
        public List<User> findUsersByString(string query)
        {
            try
            {
                List<User> res = (from item in db2.Users
                                  where item.userName.Contains(query) || item.description.Contains(query) || item.fullName.Contains(query)
                                  select item).ToList();
                return res;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            return new List<User>();
        }

        /// <summary>
        /// Find groups by query string, searches name and description
        /// </summary>
        /// <param name="query">query string</param>
        /// <returns>List of all groups found</returns>
        public List<Group> findGroupsByString(string query)
        {
            try
            {
                List<Group> res = (from item in db2.Groups
                                   where item.description.Contains(query) || item.groupName.Contains(query)
                                   select item).ToList();
                return res;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }
            return new List<Group>();
        }

        /// <summary>
        /// Fetch the newest posts from users a given userID is folllowing
        /// </summary>
        /// <param name="userID">userID of the user who wants to find newest posts form the users he follows</param>
        /// <returns>List of posts in descenting order</returns>
        public IEnumerable<Post> getRecentByFollowingUsers(string userID)
        {
            try
            {
                var posts = from user in db2.Users
                    join post in db2.Posts on user.userID equals post.userID
                    where user.Users.Select(x => x.userID).Contains(userID)
                    orderby post.dateCreated descending
                    select post;
                return posts;
            }
            catch(Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

            return null;
        }

        /// <summary>
        /// Fetch the newest groups a given userID follows
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>List of groups</returns>
        public IEnumerable<Post> getRecentFollowingGroups(string userID)
        {
            try
            {
                var groups = from g in db2.Groups
                            where g.Users.Select(x => x.userID).Contains(userID)
                            select g;
                var posts = from p in db2.Posts
                            where groups.Contains(p.Group)
                            orderby p.dateCreated descending
                            select p;

                return posts;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.Message.ToString());
            }

            return null;
        }

    }
}