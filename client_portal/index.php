<?php
session_start();

$db_file = 'database.json';
$error = '';

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $email = trim($_POST['email']);
    $password = trim($_POST['password']);

    if (file_exists($db_file)) {
        $db = json_decode(file_get_contents($db_file), true);
        
        if (isset($db['clients'][$email])) {
            $client = $db['clients'][$email];
            if (isset($client['suspended']) && $client['suspended'] === true) {
                $error = "This account has been temporarily suspended. Please contact the administrator.";
            } elseif (password_verify($password, $client['password_hash'])) {
                // Successfully Logged In
                $_SESSION['client_logged_in'] = true;
                $_SESSION['client_email'] = $email;
                $_SESSION['client_name'] = $client['name'];
                $_SESSION['client_folder'] = $client['folder'];
                
                header('Location: dashboard.php');
                exit;
            } else {
                $error = "Invalid password.";
            }
        } else {
            $error = "User not found.";
        }
    } else {
        $error = "Database error. Please contact administrator.";
    }
}
?>
<!doctype html>
<html>
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>Balaji Electro Controls Pvt. Ltd.</title>
        <link rel="stylesheet" type="text/css" href="../content/public/css/bootstrap.min.css">
        <link rel="stylesheet" href="https://unpkg.com/aos@next/dist/aos.css">
        <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" integrity="sha384-fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr" crossorigin="anonymous">
        <link rel="stylesheet" href="https://vadikom.github.io/smartmenus/src/css/sm-core-css.css">
        <link rel="stylesheet" type="text/css" href="../content/public/css/animate.css">
        <link rel="stylesheet" href="../content/public/css/sm-core-css.css">
        <link rel="stylesheet" type="text/css" href="../content/public/css/custom.css">
        <style>
            .login-wrapper { padding: 50px 0; background: #eef2f5; }
            .inner-head h1 { text-align: center; margin-bottom: 50px; color: #252b65;}
            .login-floating-form { width: 100%; background: white; padding: 40px; border-radius: 8px; box-shadow: 0 4px 15px rgba(0,0,0,0.05);}
            .login-form-group { position: relative; margin-bottom: 35px; }
            .login-form-group input { width: 100%; border: none; border-bottom: 1px solid #252b65; padding: 10px 0; font-size: 16px; background: transparent; outline: none; }
            .login-form-group label { position: absolute; left: 0; top: 10px; font-size: 16px; color: #777; pointer-events: none; transition: 0.3s ease; }
            .login-form-group input:focus + label, .login-form-group input:valid + label { top: -12px; font-size: 13px; color: #252b65; font-weight: bold;}
            .login-form-btn { text-align: center; margin-top: 20px; }
            .login-form-btn button { padding: 12px 35px; border: none; background: #252b65; color: #fff; font-size: 15px; cursor: pointer; border-radius: 4px; width: 100%; font-weight: bold;}
            .login-form-btn button:hover { background: #1d224f; }
        </style>
    </head>
    <body onload="generateCaptcha()">
        <!-- Header -->
        <div class="container-fluid fixed-onscroll m-0 p-0">
            <div class="row">
                <div class="col-lg-5 bck-white">
                    <div class="row">
                        <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12">
                            <div class="logo">
                                <a href="../index.html">
                                    <img src="../content/public/images/BEC_Logo.webp" class="img-responsive" alt="">
                                </a>
                            </div>
                        </div>
                        <div class="col-lg-7 col-md-7 col-sm-7 col-xs-12">
                            <div class="head-text-inner">
                                <h1> Trusted source for all types of electrical Control panels and solutions across all the segments since 1986. </h1>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-7 col-md-12 col-sm-7 col-xs-12">
                    <nav class="navbar navbar-expand-lg navbar-dark" role="navigation">
                        <input id="main-menu-state" type="checkbox">
                        <label class="main-menu-btn" for="main-menu-state"><span class="main-menu-btn-icon"></span> Toggle main menu</label>
                        <ul id="main-menu" class="sm sm-blue">
                            <li><a href="../index.html"><i class="fa fa-home"></i></a></li>
                            <li><a href="../balaji-electro-controls.html">BEC</a>
                                <ul>
                                    <li><a class="dropdown-item" href="../balaji-electro-controls.html">About BEC</a></li>
                                    <li><a class="dropdown-item" href="../management.html">Management</a></li>
                                    <li><a class="dropdown-item" href="../35-years-of-bec.html">35 Years of BEC</a></li>
                                    <li><a class="dropdown-item" href="../vision-and-mission.html">Vision & Mission</a></li>
                                    <li><a class="dropdown-item" href="../exports.html">Exports</a></li>
                                </ul>
                            </li>
                            <li><a href="../products.html">PRODUCTS</a></li>
                            <li><a href="../preventive-maintenance.html">SERVICES</a>
                                <ul>
                                    <li><a class="dropdown-item" href="../turnkey-solutions.html">Turnkey Solutions</a></li>
                                    <li><a class="dropdown-item" href="../preventive-maintenance.html">Preventive Maintenance</a></li>
                                    <li><a class="dropdown-item" href="../comprehensive-maintenance.html">Comprehensive Maintenance</a></li>
                                    <li><a class="dropdown-item" href="../service-on-demand.html">Service-on-demand</a></li>
                                    <li><a class="dropdown-item" href="../electrical-audits.html">Electrical Audits</a></li>
                                    <li><a class="dropdown-item" href="../electrical-engg-services.html">Electrical Engg Services</a></li>
                                </ul>
                            </li>
                            <li><a href="../design-lab.html">INFRASTRUCTURE</a>
                                <ul>
                                    <li><a class="dropdown-item" href="../design-lab.html">Design Lab</a></li>
                                    <li><a class="dropdown-item" href="../panel-manufacturing.html">Panel Manufacturing</a></li>
                                    <li><a class="dropdown-item" href="../test.html">Testing Lab</a></li>
                                </ul>
                            </li>
                            <li><a href="../quality-policy.html">QUALITY</a>
                                <ul>
                                    <li><a class="dropdown-item" href="../quality-policy.html">Quality Policy</a></li>
                                    <li><a class="dropdown-item" href="../testing.html">Testing</a></li>
                                    <li><a class="dropdown-item" href="../certificates.html">Certifications</a></li>
                                </ul>
                            </li>
                            <li><a href="../alliances.html">ALLIANCES</a></li>
                            <li><a href="../compliances.html">RESOURCES</a>
                                <ul>
                                    <li><a class="dropdown-item" href="../case-studies.html">Case Studies</a></li>
                                    <li><a class="dropdown-item" href="../compliances.html">Compliance guidelines</a></li>
                                    <li><a class="dropdown-item" href="../faq.html">FAQs</a></li>
                                </ul>
                            </li>
                            <li><a href="../clients.html">CLIENTS</a></li>
                            <li><a href="../contact-us.html">CONTACT US</a></li>
                        </ul>
                    </nav>
                </div>
            </div>
        </div>

        <div class="container-fluid car-head-back background m-0 p-0">
            <div id="carousel" class="carousel carousel-fade" data-ride="carousel" data-interval="4000">
                <div class="carousel-inner">
                    <div class="carousel-item active">
                        <div class="row">
                            <div class="col-lg-5">
                                <div class="inner-banner-left mb-0 mb-sm-3">
                                    <h2 style="font-size:24px;"><b>SECURE PORTAL</b></h2>
                                    <img src="../content/public/images/BEC_35_years_inner.webp" class="img-fluid d-none d-sm-block" alt="35years">
                                </div>
                            </div>
                            <div class="col-lg-7">
                                <div class="inner-bannerimg animated fadeInRight">
                                    <img src="../content/public/images/Contact Us.webp" class="img-fluid" alt="Banner-1">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="outer-container">
            <div class="container">
                <div class="row">
                    <div class="col-lg-12">
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="../index.html">Home</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Client Portal Login</li>
                            </ol>
                        </nav>
                    </div>
                </div>
            </div>
        </div>

        <!-- LOGIN FORM -->
        <div class="container-fluid login-wrapper">
            <div class="container">
                <div class="inner-head" data-aos="zoom-in">
                    <h1>Secure Client Login</h1>
                </div>
                <div class="row justify-content-center">
                    <div class="col-lg-5 col-md-7 col-sm-10">
                        
                        <?php if ($error): ?>
                            <div class="alert alert-danger shadow-sm text-center"><i class="fas fa-exclamation-triangle"></i> <?php echo htmlspecialchars($error); ?></div>
                        <?php endif; ?>

                        <form method="POST" class="login-floating-form">
                            <div class="login-form-group">
                                <input type="email" name="email" required>
                                <label>Email Address <span>*</span></label>
                            </div>
                            <div class="login-form-group">
                                <input type="password" name="password" required>
                                <label>Password <span>*</span></label>
                            </div>
                            <div class="login-form-btn">
                                <button type="submit"><i class="fas fa-lock"></i> Authorize & Login</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>

        <!-- FOOTER -->
        <div class="container-fluid footer-bckg mt-5">
            <div class="container">
                <div class="row">
                    <div class="col-lg-5">
                        <div class="foot-address">
                            <div class="row">
                                <div class="col-lg-3"><div class="add-img"><img src="../content/public/images/BEC_Logo-1.webp" class="img-fluid" alt="BEC_Logo-1"></div></div>
                                <div class="col-lg-9">
                                    <div class="location">
                                        <p><b>Balaji Electro Controls Pvt. Ltd.</b></p>
                                        <p>Corporate Office (Factory)</p>
                                        <p>36/2, Madanayakanahalli, Near Bhoruka School,Tumkur Road, Bengaluru - 562123, Karnataka, India</p>
                                        <p>Phone : <a href="tel:9341248803">+91 9341248803</a> / <a href="tel:9611102850">9611102850</a></p>
                                        <p>Hours : 9:00 AM to 5:30 PM</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-5">
                        <div class="foot-line"><img src="../content/public/images/Footer_Line.webp" class="img-fluid" alt="Banner_Line.webp"></div>
                        <div class="row">
                            <div class="col-lg-4"><div class="foot-links"><ul><li><a href="../contact-us.html"><img class="../content/public/images/small-arrow.webp">Contact us</a></li></ul></div></div>
                            <div class="col-lg-3"><div class="foot-links"><ul><li><a href="#">Sitemap</a></li></ul></div></div>
                            <div class="col-lg-5"><div class="foot-links"><ul><li><a href="#">Disclaimer</a></li><li><a href="#">Privacy Policy</a></li></ul></div></div>
                        </div>
                    </div>
                    <div class="col-lg-2">
                        <div class="social-media">
                            <a href="#"><i class="fab fa-linkedin-in"></i></a>
                            <a href="#"><i class="fab fa-youtube"></i></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="container-fluid footer-bckg-2">
            <div class="container">
                <div class="row">
                    <div class="col-lg-6">
                        <div class="copyright">
                            <p>&copy; 1986-2022 Balaji Electro Controls Pvt. Ltd.</p>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="copyright design">
                            <p>Design : <a href="http://lumos.in/" target="_blank"> LUMOS.in</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="container-fluid footer-bckg-3">
            <div class="container">
                <div class="row">
                    <div class="col-lg-12">
                        <a href="javascript:" id="return-to-top" class="return-to-top">
                            <h2>Back To Top</h2>
                            <img src="../content/public/images/arrow.webp" class="img-fluid" alt="Arrow_Footer">
                        </a>
                    </div>
                </div>
            </div>
        </div>

        <script src="../content/public/js/jquery-3.6.0.min.js"></script>
        <script src="../content/public/js/bootstrap.min.js"></script>
        <script src="https://unpkg.com/aos@next/dist/aos.js"></script>
        <script src="../content/public/js/jquery.smartmenus.js"></script>
        <script src="../content/public/js/custom.js"></script>
    </body>
</html>
