var pie;
var stacked;
var bar;
var column;

$(document).ready(function() {
  // JSON Object that describes how the Chart should look and where to render too
  pie = {
    chart: {
    },
    title: {
      text: 'Graphing Pie Chart'
    },
    tooltip: {
      formatter: function() {
        return '<b>'+ this.point.name +'</b>: '+ this.y;
      }
    },
    series: [{
      type: 'pie'
    }]
  };
  
  stacked = {
    chart: {
      defaultSeriesType: 'column'
    },
    title: {
      text: 'Stacked column chart'
    },
    xAxis: {
      categories: ['Apples', 'Oranges', 'Pears', 'Grapes', 'Bananas']
    },
    yAxis: {
      min: 0,
      title: {
        text: 'Total fruit consumption'
      }
    },
    tooltip: {
      formatter: function() {
        return '<b>'+ this.x +'</b><br/>'+
        this.series.name +': '+ this.y +'<br/>'+
        'Total: '+ this.point.stackTotal;
      }
    },
    plotOptions: {
      column: {
        stacking: 'normal'
      }
    },
    series: [{
      name: 'John'
    }, {
      name: 'Jane'
    }, {
      name: 'Joe'
    }]
  };
  
  bar = {
    chart: {
      defaultSeriesType: 'bar'
    },
    title: {
      text: 'Historic World Population by Region'
    },
    subtitle: {
      text: 'Source: Wikipedia.org'
    },
    xAxis: {
      categories: ['Africa', 'America', 'Asia', 'Europe', 'Oceania'],
      title: {
        text: null
      }
    },
    yAxis: {
      min: 0,
      title: {
        text: 'Population (millions)',
        align: 'high'
      }
    },
    tooltip: {
      formatter: function() {
        return ''+
        this.series.name +': '+ this.y +' millions';
      }
    },
    plotOptions: {
      bar: {
        dataLabels: {
          enabled: true
        }
      }
    },
    credits: {
      enabled: false
    },
    series: [{
      name: 'Year 1800'
    }, {
      name: 'Year 1900'
    }, {
      name: 'Year 2008'
    }]
  }
  
  column = {
    chart: {
       defaultSeriesType: 'column'
    },
    title: {
       text: 'Monthly Average Rainfall'
    },
    subtitle: {
       text: 'Source: WorldClimate.com'
    },
    xAxis: {
       categories: [
          'Jan', 
          'Feb', 
          'Mar', 
          'Apr', 
          'May', 
          'Jun', 
          'Jul', 
          'Aug', 
          'Sep', 
          'Oct', 
          'Nov', 
          'Dec'
       ]
    },
    yAxis: {
       min: 0,
       title: {
          text: 'Rainfall (mm)'
       }
    },
    tooltip: {
       formatter: function() {
          return ''+
             this.x +': '+ this.y +' mm';
       }
    },
    plotOptions: {
       column: {
          pointPadding: 0.2,
          borderWidth: 0
       }
    },
    series: [{
       name: 'Tokyo'
    }, {
       name: 'New York'
    }, {
       name: 'London'
    }, {
       name: 'Berlin'
    }]
  }
  
  // Load the Chart if it exists on the page. 
  // The loadChart plugin is in utilities.js if you need more details
  $('#pie_chart').loadChart(pie,'/data/pie.json');
  $('#stacked_chart').loadChart(stacked,'/data/stacked.json');
  $('#bar_chart').loadChart(bar,'/data/bar.json');
  $('#column_chart').loadChart(column,'/data/column.json');
});
