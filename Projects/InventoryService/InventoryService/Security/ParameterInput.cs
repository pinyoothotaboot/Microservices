using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using InventoryService.Models;
using System.Text.RegularExpressions;

namespace InventoryService.Security
{
    public class ParameterInput
    {
        public ParameterInput()
        {

        }

        public bool isValidVersion(string version)
        {
            return Regex.IsMatch(version, @"^\d*$");
        }

        public bool isValidStart_Date(string start_date)
        {
            return Regex.IsMatch(start_date, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{7}Z*$");
        }

        public bool isValidEnd_Date(string end_date)
        {
            return Regex.IsMatch(end_date, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{7}Z*$");
        }

        public bool isValidDisplay_Name(string display_name)
        {
            return Regex.IsMatch(display_name, @"^[0-9a-zA-Zก-๙-\x20]*$");
        }

        public bool isValidDescription(string description)
        {
            return Regex.IsMatch(description, @"^[0-9a-zA-Zก-๙\x20]*$");
        }

        public bool isValidCatalog_Ref_Id(string catalog_ref_id)
        {
            return Regex.IsMatch(catalog_ref_id, @"^[a-z0-9]*$");
        }

        public bool isValidAvailability_Status(string availability_status)
        {
            return Regex.IsMatch(availability_status, @"^[A-Z]*$");
        }

        public bool isValidAvailability_Date(string availability_date)
        {
            return Regex.IsMatch(availability_date, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{7}Z*$");
        }

        public bool isValidStock_Level(string stock_level)
        {
            return Regex.IsMatch(stock_level, @"^\d*$");
        }

        public bool isValidStock_Thresh(string stock_thresh)
        {
            return Regex.IsMatch(stock_thresh, @"^\d*$");
        }

        public bool isValidProduct_Id(string product_id)
        {
            return Regex.IsMatch(product_id, @"^[a-z0-9]*$");
        }

        public bool isValidCreated_By(string created_by)
        {
            return Regex.IsMatch(created_by, @"^[a-z0-9]*$");
        }

    }
}
