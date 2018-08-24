using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace C_Sharp_Regex{
    class ItemParser{
        public static int exceptionCounter{set;get;}

        public ItemParser(){
            exceptionCounter = 0;
        }

        public List<Item> rawStringToItemList(string rawData){

            string lowerCase = toLowerCase(rawData);
            string uCFirstLetter = upperCaseFirstLetter(lowerCase);
            string normal = Regex.Replace(uCFirstLetter,"[\\^%*!@]",";");
            string[] stringList = Regex.Split(normal, "##");
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
                    double pr;
                    try{
                        pr = double.Parse(array[1]);
                    } catch(System.FormatException){
                        pr = 0;
                    }
                    item.price = pr;
                } else if(Regex.IsMatch(array[0],"type")){
                    item.type = array[1];
                } else if(Regex.IsMatch(array[0],"expiration")){
                    item.expirationDate = array[1];
                }
           }
           return item;
        }

        public string toLowerCase(string input){
            string alphabetLower = "a b c d e f g h i j k l m n o p q r s t u v w x y z";
            string[] lower = Regex.Split(alphabetLower," ");
            string alphabetUpper = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
            string[] upper = Regex.Split(alphabetUpper, " ");
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
           Dictionary<string,int> names = new Dictionary<string, int>();
           string groceryList = "";

           foreach(Item item in list){
               int x = countNameOccurrences(list,item.name);
               if(!names.ContainsKey(item.name)){
                   names.Add(item.name,x);
               }
           }
           foreach(KeyValuePair<string,int> pair in names){
               Dictionary<double,int> prices = new Dictionary<double, int>();
               foreach(Item item in list){
                   if(pair.Key == item.name && !prices.ContainsKey(item.price)){
                       int y = countPriceOccurrences(list,item.name,item.price);
                       prices.Add(item.price,y);
                   }
               }

               groceryList += "name: " + pair.Key + "    seen: " + pair.Value + "\n";
               foreach(KeyValuePair<double,int> prc in prices){
                   groceryList += "price: " + prc.Key + "   seen: " + prc.Value + "\n";
               }
               groceryList += "\n";
               
           }
           groceryList += "errors: " + exceptionCounter;
           return groceryList;
        }

        private int countNameOccurrences(List<Item> list, string name){
            int x = 0;
            foreach(Item item in list){
                if(name == item.name){
                    x++;
                }
            }
            return x;
        }

        private int countPriceOccurrences(List<Item> list, string name, double price){
            int x = 0;
            foreach(Item item in list){
                if(item.name == name && item.price == price){
                    x++;
                }
            }
            return x;
        }


        public string printParsedList(List<Item> list){
            string id = "";
            foreach(Item item in list){
                id += item.toString();
            }
            return id;
        }

    }
}