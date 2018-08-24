using System;

namespace C_Sharp_Regex{
    class Item{

        public string name{get;set;}
        public double price{get;set;}
        public string type{get;set;}
        public string expirationDate{get;set;}

        public Item(string name, double price, string type, string expirationDate){
            this.name = name;
            this.price = price;
            this.type = type;
            this.expirationDate = expirationDate;
        }       

        public string toString(){
            return "name: " + name + ", price: " + price + ", type: " + type + ", expiration: " + expirationDate + "\n";
        } 
    }
}