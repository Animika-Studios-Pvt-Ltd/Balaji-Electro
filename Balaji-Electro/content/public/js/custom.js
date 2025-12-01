// ===== Scroll to Top ==== 
$(window).scroll(function() {
    if ($(this).scrollTop() >= 50) {       
        $('#return-to-top').fadeIn(200);    
    } else {
        $('#return-to-top').fadeOut(200);   
    }
});

$('#return-to-top').click(function() {     
    $('body,html').animate({
        scrollTop : 0                      
    }, 500);
});
 

// SmartMenus init
    $(function() {
        $('#main-menu').smartmenus({
            subMenusSubOffsetX: 1,
            subMenusSubOffsetY: -2
        });
    });

    // SmartMenus mobile menu toggle button
    $(function() {
        var $mainMenuState = $('#main-menu-state');
        if ($mainMenuState.length) {

            $mainMenuState.change(function(e) {
                var $menu = $('#main-menu');
                if (this.checked) {
                    $menu.hide().slideDown(1000, function() { $menu.css('display', ''); });
                }
                else {
                    $menu.show().slideUp(1000, function() { $menu.css('display', ''); });
                }
            });

            $(window).bind('beforeunload unload', function() {
                if ($mainMenuState[0].checked) {
                    $mainMenuState[0].click();
                }
            });
        }
    });


$(window).on("scroll", function() {
    if($(window).scrollTop() > 50) {
        $(".fixed-onscroll").addClass("active");
    } else {
        //remove the background property so it comes transparent again (defined in your css)
       $(".fixed-onscroll").removeClass("active");
    }
});


//Animation on scroll 
AOS.init();

$(document).ready(function(){

    $("#contactLink").click(function(){
      if ($("#contactForm").is(":hidden")){
        $("#contactForm").slideDown(2000);
    }
    else{
      $("#contactForm").slideUp(2000);
    }
  });          
  });
            
  function closeForm2() {
    $("#contactForm").slideUp(2000);
}

// function toggleIcon(e) {
//     $(e.target)
//         .prev('.panel-heading')
//         .find(".more-less")
//         .toggleClass('glyphicon-plus glyphicon-minus');
// }
// $('.panel-group').on('hidden.bs.collapse', toggleIcon);
// $('.panel-group').on('shown.bs.collapse', toggleIcon);

// var acc = document.getElementsByClassName("accordion");
// var i;

// for (i = 0; i < acc.length; i++) {
//   acc[i].addEventListener("click", function() {
//     this.classList.toggle("active");
//     var panel = this.nextElementSibling;
//     if (panel.style.maxHeight) {
//       panel.style.maxHeight = null;
//     } else {
//       panel.style.maxHeight = panel.scrollHeight + "px";
//     } 
//   });
// }

$(document).ready(function(){

    $("#para").click(function(){
      if ($("#contactForm").is(":hidden")){
        $("#contactForm").slideDown(2000);
    }
    else{
      $("#contactForm").slideUp(2000);
    }
  });          
  });
            
  function closeForm2() {
    $("#contactForm").slideUp(2000);
}

$('.counter-count').each(function () {
        $(this).prop('Counter',0).animate({
            Counter: $(this).text()
        }, {
            duration: 5000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });


/*let rightCode = '';
let valiIpt = document.querySelector('#valiIpt');
let toggleBtn = document.querySelector('#toggle');
let submitBtn = document.querySelector('#submit');
toggleBtn.addEventListener('click', function(){
  getImgValiCode();
  console.log('click:' + rightCode);
}, false);
submitBtn.addEventListener('click', function(){
  if (valiIpt.value === '') {
    alert('verification code must be filled!');
    return false;
  }
  if (valiIpt.value !== rightCode) {
    alert('Verification code error!');
    return false;
  }
  alert('Submitted successfully!')
  valiIpt.value = '';
}, false);
getImgValiCode();
console.log('init:' + rightCode);
function getImgValiCode () {
  let showNum = [];
  let canvasWinth = 150;
  let canvasHeight = 30;
  let canvas = document.getElementById('valicode');
  let context = canvas.getContext('2d');
  canvas.width = canvasWinth;
  canvas.height = canvasHeight;
  let sCode = 'A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,0,1,2,3,4,5,6,7,8,9,!,@,#,$,%,^,&,*,(,)';
  let saCode = sCode.split(',');
  let saCodeLen = saCode.length;
  for (let i = 0; i <= 3; i++) {
    let sIndex = Math.floor(Math.random()*saCodeLen);
    let sDeg = (Math.random()*30*Math.PI) / 180;
    let cTxt = saCode[sIndex];
    showNum[i] = cTxt.toLowerCase();
    let x = 10 + i*20;
    let y = 20 + Math.random()*8;
    context.font = 'bold 23px 微软雅黑';
    context.translate(x, y);
    context.rotate(sDeg);

    context.fillStyle = randomColor();
    context.fillText(cTxt, 0, 0);

    context.rotate(-sDeg);
    context.translate(-x, -y);
  }
  for (let i = 0; i <= 5; i++) {
    context.strokeStyle = randomColor();
    context.beginPath();
    context.moveTo(
      Math.random() * canvasWinth,
      Math.random() * canvasHeight
    );
    context.lineTo(
      Math.random() * canvasWinth,
      Math.random() * canvasHeight
    );
    context.stroke();
  }
  for (let i = 0; i < 30; i++) {
    context.strokeStyle = randomColor();
    context.beginPath();
    let x = Math.random() * canvasWinth;
    let y = Math.random() * canvasHeight;
    context.moveTo(x,y);
    context.lineTo(x+1, y+1);
    context.stroke();
  }
  rightCode = showNum.join('');
}

function randomColor () {
  let r = Math.floor(Math.random()*256);
  let g = Math.floor(Math.random()*256);
  let b = Math.floor(Math.random()*256);
  return 'rgb(' + r + ',' + g + ',' + b + ')';
}*/



/*function Captcha(){
                     var alpha = new Array('A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z');
                     var i;
                     for (i=0;i<6;i++){
                       var a = alpha[Math.floor(Math.random() * alpha.length)];
                       var b = alpha[Math.floor(Math.random() * alpha.length)];
                       var c = alpha[Math.floor(Math.random() * alpha.length)];
                       var d = alpha[Math.floor(Math.random() * alpha.length)];
                       var e = alpha[Math.floor(Math.random() * alpha.length)];
                       var f = alpha[Math.floor(Math.random() * alpha.length)];
                       var g = alpha[Math.floor(Math.random() * alpha.length)];
                      }
                    var code = a + ' ' + b + ' ' + ' ' + c + ' ' + d + ' ' + e + ' '+ f + ' ' + g;
                    document.getElementById("mainCaptcha").value = code
                  }
                  function ValidCaptcha(){
                      var string1 = removeSpaces(document.getElementById('mainCaptcha').value);
                      var string2 = removeSpaces(document.getElementById('txtInput').value);
                      if (string1 == string2){
                        return true;
                      }
                      else{        
                        return false;
                      }
                  }
                  function removeSpaces(string){
                    return string.split(' ').join('');
                  }*/


function generateCaptcha()
         {
             var alpha = new Array('A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z');
             var i;
             for (i=0;i<4;i++){
               var a = alpha[Math.floor(Math.random() * alpha.length)];
               var b = alpha[Math.floor(Math.random() * alpha.length)];
               var c = alpha[Math.floor(Math.random() * alpha.length)];
               var d = alpha[Math.floor(Math.random() * alpha.length)];
              }
            var code = a + '' + b + '' + '' + c + '' + d;
            document.getElementById("mainCaptcha").value = code
          }
          function CheckValidCaptcha(){
              var string1 = removeSpaces(document.getElementById('mainCaptcha').value);
              var string2 = removeSpaces(document.getElementById('txtInput').value);
              if (string1 == string2){
         document.getElementById('success').innerHTML = "Valid Captcha";
         //alert("Form is validated Successfully");
                return true;
              }
              else{       
         document.getElementById('error').innerHTML = "Please enter a valid captcha."; 
         //alert("Please enter a valid captcha.");
                return false;
         
              }
          }
          function removeSpaces(string){
            return string.split(' ').join('');
          } 
