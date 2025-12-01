<?php
if(!empty($_REQUEST['Name']) && !empty($_REQUEST['Email']) && !empty($_REQUEST['Phone'])){
    $name = $_REQUEST['Name'];
    echo $email = $_REQUEST['Email'];
    echo $fname=explode(" ", $name);
    echo $phone  = $_REQUEST['Phone'];
    echo $mes  = $_REQUEST['Message'];
    echo $text=$_REQUEST['Company'];
    
    // ini_set("SMTP", "mail.milkywave.in");
    ini_set("sendmail_from", "info@lumos.in");
    
    $to="vidya@lumos.in";
    $headers = "MIME-Version: 1.0" . "\r\n";
    $headers .= "Content-type:text/html;charset=UTF-8" . "\r\n";
    $headers .= 'From:info@lumos.in <info@lumos.in>' . "\r\n";
    
    $headers .= 'CC: vidya@lumos.in' . "\r\n";
    
    $headers1 = "MIME-Version: 1.0" . "\r\n";
    $headers1 .= "Content-type:text/html;charset=UTF-8" . "\r\n";
    $headers1 .= 'From:info@lumos.in <info@lumos.in>' . "\r\n";

    $subject1 = "[Balaji Electro Controls Pvt. Ltd.] - You have a new Business enquiry!";
    $subject2 = "[Balaji Electro Controls Pvt. Ltd.] - Thanks for contacting us!";
    $message1 = "<html>
    <head>
    <title></title>
    </head>
    <body>
    Dear Team,<br/><br/>
    You have a new business enquiry.<br/><br/>
    Name: $name<br/>
    Phone: $phone<br/>
    Email: $email<br/>
    Message: $mes<br/>
    EnquiryType: $type<br/><br/>
    Regards,<br/>
    Balaji Electro Controls Pvt. Ltd.
    
    </body>
    </html>";

     $message2 = "<html>
    <head>
    <title></title>
    </head>
    <body>
    Hi $fname[0],<br/><br/>
    Thank you for contacting us. we will get back to you as soon as possible.<br/>
    <br/><br/>
    Regards,<br/>
    Balaji Electro Controls Pvt. Ltd.

    
    </body>
    </html>";
    // echo $message2;die;
    mail($email,$subject2,$message2,$headers1);
    mail($to,$subject1,$message1,$headers);
        
     //header('Location: Thank-You.html');
      
     
     ?>
     
     <meta http-equiv="refresh" content="0;URL=thank-you.html" />
     
<?php    }
     
 ?>