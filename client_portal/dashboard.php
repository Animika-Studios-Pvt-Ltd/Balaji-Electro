<?php
session_start();

// Ensure client is logged in
if (!isset($_SESSION['client_logged_in'])) {
    header('Location: index.php');
    exit;
}

// Active suspension check
$db_file = 'database.json';
$is_suspended = false;
$client_deleted = true;

if (isset($_SESSION['client_email']) && file_exists($db_file)) {
    $db = json_decode(file_get_contents($db_file), true);
    if (isset($db['clients'][$_SESSION['client_email']])) {
        $client_deleted = false;
        if (isset($db['clients'][$_SESSION['client_email']]['suspended']) && $db['clients'][$_SESSION['client_email']]['suspended'] === true) {
            $is_suspended = true;
        }
    }
}

if ($client_deleted || $is_suspended) {
    unset($_SESSION['client_logged_in']);
    header('Location: index.php');
    exit;
}

$client_name = $_SESSION['client_name'];
$folder_name = $_SESSION['client_folder'];
$doc_dir = 'client_documents/' . $folder_name . '/';

// Handle File Download securely
if (isset($_GET['download'])) {
    $file = basename($_GET['download']); 
    $filepath = $doc_dir . $file;
    
    if (file_exists($filepath) && !is_dir($filepath)) {
        require_once 'logger.php';
        log_action('DOWNLOAD', 'Client: ' . $_SESSION['client_name'], 'File: ' . $file, 'Client initiated a secure file download.');
        
        header('Content-Description: File Transfer');
        header('Content-Type: application/octet-stream');
        header('Content-Disposition: attachment; filename="' . basename($filepath) . '"');
        header('Expires: 0');
        header('Cache-Control: must-revalidate');
        header('Pragma: public');
        header('Content-Length: ' . filesize($filepath));
        readfile($filepath);
        exit;
    } else {
        $error = "File not found.";
    }
}

// Fetch database info to check for suspended files
$suspended_files = [];
$db_file = 'database.json';
if (file_exists($db_file)) {
    $db = json_decode(file_get_contents($db_file), true);
    if (isset($db['clients'][$_SESSION['client_email']]['suspended_files'])) {
        $suspended_files = $db['clients'][$_SESSION['client_email']]['suspended_files'];
    }
}

// Read contents of the client's folder
$files = [];
if (is_dir($doc_dir)) {
    $scanned = array_diff(scandir($doc_dir), array('..', '.'));
    foreach ($scanned as $item) {
        if (!is_dir($doc_dir . $item) && !in_array($item, $suspended_files)) {
            $files[] = [
                'name' => $item,
                'size' => round(filesize($doc_dir . $item) / 1024, 2) . ' KB',
                'time' => date("Y-m-d H:i", filemtime($doc_dir . $item))
            ];
        }
    }
}

// Handle Logout
if (isset($_GET['logout'])) {
    unset($_SESSION['client_logged_in']);
    header('Location: index.php');
    exit;
}
?>
<!doctype html>
<html>
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>Client Dashboard - Balaji Electro Controls Pvt. Ltd.</title>
        <link rel="stylesheet" type="text/css" href="../content/public/css/bootstrap.min.css">
        <link rel="stylesheet" href="https://unpkg.com/aos@next/dist/aos.css">
        <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" integrity="sha384-fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr" crossorigin="anonymous" media="screen">
        <link rel="stylesheet" href="https://vadikom.github.io/smartmenus/src/css/sm-core-css.css">
        <link rel="stylesheet" type="text/css" href="../content/public/css/animate.css">
        <link rel="stylesheet" href="../content/public/css/sm-core-css.css">
        <link rel="stylesheet" type="text/css" href="../content/public/css/custom.css">
        <style>
            .dashboard-wrapper { padding: 50px 0; background: #f8f9fa; }
            .card { border: none; box-shadow: 0 5px 15px rgba(0,0,0,0.05); border-radius: 10px; overflow: hidden; }
            .card-header { background: white; border-bottom: 2px solid #e9ecef; padding: 25px 30px; }
            .card-header h3 { margin: 0; color: #252b65; font-weight: 600; }
            .table { margin-bottom: 0; }
            .table thead th { background: #f8f9fa; border-top: none; color: #495057; font-weight: 600; text-transform: uppercase; font-size: 13px; padding: 15px 30px; letter-spacing: 0.5px;}
            .table tbody td { padding: 20px 30px; vertical-align: middle; color: #333;}
            .file-icon { color: #dc3545; font-size: 24px; margin-right: 15px; vertical-align: middle; }
            .download-btn { background: #252b65; color: white; padding: 8px 20px; text-decoration: none; border-radius: 4px; display: inline-block; transition: 0.3s;}
            .download-btn:hover { background: #1d224f; color: white; text-decoration: none; box-shadow: 0 3px 6px rgba(0,0,0,0.1);}
            .empty-state { text-align: center; padding: 60px 20px; color: #6c757d; }
            .empty-state i { font-size: 50px; margin-bottom: 20px; color: #dee2e6; }
            .logout-btn { background: white; color: #dc3545; border: 1px solid #dc3545; padding: 8px 20px; border-radius: 4px; text-decoration: none; transition: 0.3s; }
            .logout-btn:hover { background: #dc3545; color: white; text-decoration: none;}
        </style>
    </head>
    <body>
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
                        <label class="main-menu-btn" for="main-menu-state"><span class="main-menu-btn-icon"></span></label>
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
                                    <h2 style="font-size:24px;"><b>CLIENT PORTAL</b></h2>
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
                                <li class="breadcrumb-item active" aria-current="page">Client Portal</li>
                            </ol>
                        </nav>
                    </div>
                </div>
            </div>
        </div>

        <!-- MAIN DASHBOARD CONTENT -->
        <div class="dashboard-wrapper">
            <div class="container">
                <div class="row">
                    <div class="col-12">
                        
                        <?php if (isset($error)): ?>
                            <div class="alert alert-danger shadow-sm"><?php echo htmlspecialchars($error); ?></div>
                        <?php endif; ?>

                        <div class="card mb-5">
                            <div class="card-header d-flex justify-content-between align-items-center">
                                <div>
                                    <h3>Welcome back, <?php echo htmlspecialchars($client_name); ?>!</h3>
                                    <p class="text-muted mb-0 mt-2">Here are the secure documents provided exclusively for you.</p>
                                </div>
                                <a href="?logout=1" class="logout-btn"><i class="fas fa-sign-out-alt"></i> Logout</a>
                            </div>
                            <div class="card-body p-0">
                                <?php if (count($files) > 0): ?>
                                <div class="table-responsive">
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th>Document Details</th>
                                                <th>Added On</th>
                                                <th>Size</th>
                                                <th class="text-right">Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <?php foreach ($files as $file): ?>
                                            <tr>
                                                <td>
                                                    <i class="fas fa-file-pdf file-icon"></i>
                                                    <strong><?php echo htmlspecialchars($file['name']); ?></strong>
                                                </td>
                                                <td><?php echo $file['time']; ?></td>
                                                <td><?php echo $file['size']; ?></td>
                                                <td class="text-right">
                                                    <a href="?download=<?php echo urlencode($file['name']); ?>" class="download-btn">
                                                        <i class="fas fa-download"></i> Download
                                                    </a>
                                                </td>
                                            </tr>
                                            <?php endforeach; ?>
                                        </tbody>
                                    </table>
                                </div>
                                <?php else: ?>
                                <div class="empty-state">
                                    <i class="fas fa-folder-open"></i>
                                    <h4>No Documents Available</h4>
                                    <p>There are no documents currently uploaded to your secure folder.</p>
                                </div>
                                <?php endif; ?>
                            </div>
                        </div>
                        
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
