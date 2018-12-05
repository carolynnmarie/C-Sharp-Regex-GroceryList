using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace C_Sharp_Regex{
    class ItemParser{
        public static int exceptionCounter{set;get;}

        public ItemParser(){
            exceptionCounter = 0;
        }

        public List<Item> rawStringToItemList(string rawData){

            string normalized = toLowerCase(rawData);
            normalized = upperCaseFirstLetter(normalized);
            normalized = Regex.Replace(normalized,"[\\^%*!@]",";");
            string[] stringList = Regex.Split(normalized, "##");
            return stringListToItemList(stringList);
        }

        public List<Item> stringListToItemList(string[] list){
            List<Item> itemList = new List<Item>();
            foreach(string str in list){
                Item item = stringToItemObject(str);
                if(item.name!="" && item.price!=0 && item.type !="" && item.expirationDate!= ""){
                    itemList.Add(item);
                } else{
                    exceptionCounter++;
                }
            }
            return itemList;
        }

        public Item stringToItemObject(string itemString){
            string[] kVPairs = Regex.Split(itemString,";");
            Item item = new Item("",0,"","");
            foreach(string id in kVPairs){
                string[] array = Regex.Split(id,":");
                if(Regex.IsMatch(array[0],"name")){
                    item.name = array[1];
                } else if(Regex.IsMatch(array[0],"price")){
                    try{
                        item.price = double.Parse(array[1]);
                    } catch(System.FormatException){
                        item.price = 0;
                    }
                } else if(Regex.IsMatch(array[0],"type")){
                    item.type = array[1];
                } else if(Regex.IsMatch(array[0],"expiration")){
                    item.expirationDate = array[1];
                }
           }
           return item;
        }

        public string toLowerCase(string input){
            string[] lower = Regex.Split("a b c d e f g h i j k l m n o p q r s t u v w x y z", " ");
            string[] upper = Regex.Split("A B C D E F G H I J K L M N O P Q R S T U V W X Y Z", " ");
            for(int i = 0; i<lower.Length; i++){
                Regex rgx = new Regex(upper[i]);
                input = rgx.Replace(input,lower[i]);
           }
            return input;
        }

        public string upperCaseFirstLetter(string input){
            string [] lowerArray = {"milk","bread","cookies","apples","food"};
            string [] upperArray = {"Milk","Bread","Cookies","Apples","Food"};
            for(int i = 0; i<lowerArray.Length; i++){
                if(Regex.IsMatch(input,lowerArray[i])){
                    input = Regex.Replace(input,lowerArray[i],upperArray[i]);
                }
            }
            return input;
        }

        public string printGroceryList(List<Item> list){
           StringBuilder groceryList = new StringBuilder();
           Dictionary<string,int> names = nameOccurrences(list);
           foreach(KeyValuePair<string,int> pair in names){
               Dictionary<double,int> prices = itemPrices(pair,list);
               groceryList.Append(String.Format("name: {0,7}     seen: {1} times\n", pair.Key, pair.Value))
                          .Append("=============     =============\n");
               foreach(KeyValuePair<double,int> prc in prices){
                   groceryList.Append(String.Format("price:  ${0,4}     seen: {1} times\n", prc.Key, prc.Value))
                              .Append("-------------     -------------\n");
               }
               groceryList.Append("\n");
           }
           groceryList.Append(String.Format("{0,-18}seen: {1} times", "errors:", exceptionCounter));
           return groceryList.ToString();
        }

        private Dictionary<string, int> nameOccurrences(List<Item>list){
            Dictionary<string,int> names = new Dictionary<string, int>();
            foreach(Item item in list){
               int x = countNameOccurrences(list, item.name);
               if(!names.ContainsKey(item.name)) names.Add(item.name, x);
           }
           return names;
        }

        private int countNameOccurrences(List<Item> list, string name){
            int x = 0;
            foreach(Item item in list){
                if(Regex.IsMatch(item.name,name))x++;
            }
            return x;
        }

        private Dictionary<double, int> itemPrices(KeyValuePair<string,int> pair,List<Item>list){
            Dictionary<double,int> prices = new Dictionary<double, int>();
            foreach(Item item in list){
                int y = countPriceOccurrences(list, item);
                if(pair.Key == item.name && !prices.ContainsKey(item.price)){
                    prices.Add(item.price, y);
               }
           }
           return prices;
        }

        private int countPriceOccurrences(List<Item> list, Item groceryItem){
            int x = 0;
            foreach(Item item in list){
                if(Regex.IsMatch(item.name, groceryItem.name) && item.price == groceryItem.price) x++;
            }
            return x;
        }

    }
}
