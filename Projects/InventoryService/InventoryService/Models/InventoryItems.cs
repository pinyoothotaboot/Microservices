using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace InventoryService.Models
{
    public class InventoryItems
    {   
        [BsonId]
        [BsonRequired]
        public ObjectId Id { get; protected set; }

        [BsonElement("version")]
        [BsonRequired]
        public int Version { get; protected set; }

        [BsonElement("creation_date")]
        [BsonRequired]
        public DateTime Creation_Date { get; protected set; }

        [BsonElement("start_date")]
        public DateTime Start_Date { get; protected set; }

        [BsonElement("end_date")]
        public DateTime End_Date { get; protected set; }

        [BsonElement("display_name")]
        [BsonRequired]
        public string Display_Name { get; protected set; }

        [BsonElement("description")]
        public string Description { get; protected set; }

        [BsonElement("catalog_ref_id")]
        [BsonRequired]
        public ObjectId Catalog_Ref_Id { get; protected set; }

        [BsonElement("availability_status")]
        [BsonRequired]
        public string Availability_Status { get; protected set; }

        [BsonElement("availability_date")]
        [BsonRequired]
        public DateTime Availability_Date { get; protected set; }

        [BsonElement("stock_level")]
        [BsonRequired]
        public int Stock_Level { get; protected set; }

        [BsonElement("stock_thresh")]
        [BsonRequired]
        public int Stock_Thresh { get; protected set; }

        [BsonElement("product_id")]
        [BsonRequired]
        public ObjectId Product_Id { get; protected set; }

        [BsonElement("created_by")]
        [BsonRequired]
        public ObjectId Created_By { get; protected set; }

        [BsonElement("updated_by")]
        public ObjectId Updated_By { get; protected set; }

        [BsonElement("updated_date")]
        public DateTime Updated_Date { get; protected set; }

        [BsonElement("visible")]
        [BsonRequired]
        public bool Visible { get; protected set; }

       
        protected InventoryItems()
        {
        }

        public InventoryItems(int version,DateTime start_date,DateTime end_date,
                              string display_name,string description,string catalog_ref_id,
                              string availability_status,DateTime availability_date,
                              int stock_level,int stock_thresh,string product_id,
                              string created_by)
        {

            if (string.IsNullOrWhiteSpace(display_name))
            {
                throw new Exception("Inventory display name can't be empty.");
            }

            if (string.IsNullOrWhiteSpace(catalog_ref_id))
            {
                throw new Exception("Inventory catalog ref id can't be empty.");
            }

            if (string.IsNullOrWhiteSpace(availability_status))
            {
                throw new Exception("Inventory availability status can't be empty.");
            }

            if (string.IsNullOrWhiteSpace(product_id))
            {
                throw new Exception("Inventory product id can't be empty.");
            }

            if (string.IsNullOrWhiteSpace(created_by))
            {
                throw new Exception("Inventory created by can't be empty.");
            }

            Id = ObjectId.GenerateNewId();
            Version = version;
            Creation_Date = DateTime.UtcNow;
            Start_Date = start_date;
            End_Date = end_date;
            Display_Name = display_name;
            Description = description;
            Catalog_Ref_Id = ObjectId.Parse(catalog_ref_id);
            Availability_Status = availability_status;
            Availability_Date = availability_date;
            Stock_Level = stock_level;
            Stock_Thresh = stock_thresh;
            Product_Id = ObjectId.Parse(product_id);
            Created_By = ObjectId.Parse(created_by);
            Visible = true;
        }

    }
}
