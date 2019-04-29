using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace MongWebApplication.Controllers
{
    public class HomeController : ApiController
    {
        private static string connStr = "mongodb://127.0.0.1:27017/?safe=true";
        //创建或打开数据库
        IMongoDatabase database = new MongoClient(connStr).GetDatabase("test");
        //var database = client.GetDatabase("test");

        /// <summary>
        /// 添加/初始化数据
        /// </summary>
        [HttpPost]
        public string Create(News input)
        {
            var collectionNewsTitle = database.GetCollection<News>("News");
            collectionNewsTitle.InsertOne(new News { Title = input.Title, Content = input.Content, Date = DateTime.Now.ToString() });
            return "添加数据成功..";
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var collectionNews = database.GetCollection<News>("News");
            var list = collectionNews.Find(Builders<News>.Filter.Empty).ToList();
            return Json<List<News>>(list);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Update(News input)
        {
            var collectionNews = database.GetCollection<News>("News");
            //var filter = Builders<News>.Filter.Eq("_id", input._id);
            var filter = Builders<News>.Filter.Eq("Title", input.Title);
            var update = Builders<News>.Update.Set("Content", input.Content).Set("Date", DateTime.Now.ToString());
            var result = collectionNews.UpdateOne(filter, update);
            return collectionNews.Find(Builders<News>.Filter.Eq("_id", input._id)).ToString();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string Delete(string title)
        {
            var collectionNews = database.GetCollection<News>("News");
            //collectionNews.DeleteOne(Builders<News>.Filter.Eq("_id", id));
            collectionNews.DeleteOne(Builders<News>.Filter.Eq("Title", title));
            return "删除成功..";
        }
    }

    public class News
    {
        public ObjectId _id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Date { get; set; }
    }
}
