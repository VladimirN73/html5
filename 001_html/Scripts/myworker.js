onmessage = function(e){
  //var work = 1000000;
  var work = e.data;
  self.postMessage("worker input value: "+ work + ". Calculation is running ...");
  var i = 0;
  var a = new Array(work);
  var sum = 0;
  for (i = 0; i<work; i++)
  {
    a[i]= i*i;
    sum += i *i;
    for (j = 0; j< work; j++)
    {
      var xxx = j*j;
    } 
  }
  self.postMessage(sum);
}