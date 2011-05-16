 function getDateFormat(local)
        {
            
            var currentDate;
            var nday;
            var day;
            var month;
            var year;
            var monthName;
            var currentTime;
            var hours;
            var minutes;
            var seconds;
            var UTC;
            
            if(local==true)
            {
                currentDate = new Date();
                nday = currentDate.getDay();
                day = currentDate.getDate();                
                month = currentDate.getMonth()+1;
                year = currentDate.getFullYear();
                monthName;
                currentTime = new Date();
                hours = currentTime.getHours();
                minutes = currentTime.getMinutes();
                seconds=currentTime.getSeconds();                
           }
           
           if(local==false)
            {
                currentDate =new Date();
                nday = currentDate.getUTCDay();
                day = currentDate.getUTCDate();
                month = currentDate.getUTCMonth()+1;
                year = currentDate.getUTCFullYear();
                monthName;
                currentTime = new Date();
                hours = currentTime.getUTCHours();
                minutes = currentTime.getUTCMinutes();
                seconds=currentTime.getUTCSeconds();                
           }
         
           
              
            if(day==31)
            {
            var changedDate=month + "-"+ day+ "-" + year+" "+ hours + ":" + minutes + ":"+seconds+" "+ suffix;
            var newDate=new Date(year,month,0);
            day=newDate.getDate();
            }
          
           
        
            if (nday==0)
              nday="Sun";
            if (nday==1)
              nday="Mon";
            if (nday==2)
              nday="Tue";
            if (nday==3)
              nday="Wed";
            if (nday==4)
              nday="Thu";
            if (nday==5)
              nday="Fri";
            if (nday==6)
              nday="Sat";
        
            if(month==1)
                monthName="Jan";
            if(month==2)
                monthName="Feb";
            if(month==3)
                monthName="Mar";
            if(month==4)
                monthName="Apr";
            if(month==5)
                monthName="May";
            if(month==6)
                monthName="Jun";
            if(month==7)
                monthName="Jul";
            if(month==8)
                monthName="Aug";
            if(month==9)
                monthName="Sep";
            if(month==10)
                monthName="Oct";
            if(month==11)
                monthName="Nov";
            if(month==12)
                monthName="Dec";

            var suffix = "";
            if (local == true) {
                suffix = "AM";
                if (hours >= 12) {
                    suffix = "PM";
                    hours = hours - 12;
                }

                if (hours == 0) {
                    hours = 12;
                }
            }


            if (minutes < 10)
            minutes = "0" + minutes
            
            if (seconds < 10)
            seconds = "0" + seconds
            
            var dateNtime=nday + ", "+ day+ " " +monthName+" "+ year+" "+ hours + ":" + minutes + ":"+seconds+" "+ suffix;
            
            return dateNtime;
            
        }

 		function getTimeFormat(local)
        {
            var currentTime;
            var hours;
            var minutes;
            var seconds;
            var UTC;
            
            if(local==true)
            {
                currentTime = new Date();
                hours = currentTime.getHours();
                minutes = currentTime.getMinutes();
                seconds=currentTime.getSeconds();                
           }
           
           if(local==false)
           {
                currentTime = new Date();
                hours = currentTime.getUTCHours();
                minutes = currentTime.getUTCMinutes();
                seconds=currentTime.getUTCSeconds();                
           }
            var suffix = "";
            if (local == true) {
                suffix = "AM";
                if (hours >= 12) {
                    suffix = "PM";
                    hours = hours - 12;
                }

                if (hours == 0) {
                    hours = 12;
                }
            }


            if (minutes < 10)
            minutes = "0" + minutes
            
            if (seconds < 10)
            seconds = "0" + seconds
            
            var time = hours + ":" + minutes + ":"+seconds+" "+ suffix;
            
            return time;
            
        }
