 function getDate(m,d,h)
        {
            var currentDate = new Date()
            var day = currentDate.getDate()
            var month = currentDate.getMonth()+1
            var year = currentDate.getFullYear()
            
            var currentTime = new Date()
            var hours = currentTime.getHours()
            var minutes = currentTime.getMinutes()
            var seconds=currentTime.getSeconds();
            
            if(h!="")
            {
              hours=hours-h;  
              minutes=minutes-minutes;
            }
            if(m!="")
            {
                month=month-m;
                if(day==31)
                {
                //var changedDate=month + "-"+ day+ "-" + year+" "+ hours + ":" + minutes + ":"+seconds+" "+ suffix;
                var changedDate=year+ "/"+ month+ "/" + day +" "+ hours + ":" + minutes + ":"+seconds+" "+ suffix;
                var newDate=new Date(year,month,0);
                day=newDate.getDate();
                }
            }
            if(d!="")
            {
               
                if(day < 8)
                {
                    month = month -1;   
                    var dayPrev=caldays(month,year);                    
                    day = dayPrev - (d- day);
                    
                }
                else
                {
                    day=day-d;
                }
                
            }

            var suffix = "AM";
            if (hours >= 12) {
            suffix = "PM";
            hours = hours - 12;
                             }
            if (hours == 0) {
            hours = 12;
            }

            if (minutes < 10)
            minutes = "0" + minutes
            
            if (seconds < 10)
            seconds = "0" + seconds
            
            if(h == -24)
            {
                hours = 11;
                minutes = 59;
                suffix= "PM";
            }
            
            //var dateNtime=month + "-"+ day+ "-" + year+" "+ hours + ":" + minutes + ":"+seconds+" "+ suffix;
            var dateNtime=year+ "/"+ month+ "/" + day+" "+ hours + ":" + minutes + ":"+seconds+" "+ suffix;
            return dateNtime;
            
        }
        
Date.prototype.addMonths = function(n)
{
this.setMonth(this.getMonth()+n);
return this;
}


function caldays(m,y)
{

        if(m==01||m==03||m==05||m==07||m==08||m==10||m==12)
	{
		var dmax = 31;			
		return dmax;	        

	}
	else if (m==04||m==06||m==09||m==11)
	{

        var dmax = 30;		
		return dmax;		  

	}
	else
	{

		if((y%400==0) || (y%400==0 && y%100!=0))
		{

			dmax = 29;			
			return dmax;

		}
                else 
                {
                    dmax = 28;				
                }
		return dmax;

	}

}