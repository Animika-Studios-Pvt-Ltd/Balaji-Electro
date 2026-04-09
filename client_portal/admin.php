<?php
session_start();

// ====== Configuration ======
$db_file = 'database.json';
$doc_dir = 'client_documents/';

if (!is_dir($doc_dir)) {
    mkdir($doc_dir, 0755, true);
}

// ------ Handle Logout ------
if (isset($_GET['logout'])) {
    unset($_SESSION['admin_logged_in']);
    header('Location: admin.php');
    exit;
}

$error = '';
$success = '';

// Check for flash messages
if (isset($_SESSION['flash_success'])) {
    $success = $_SESSION['flash_success'];
    unset($_SESSION['flash_success']);
}
if (isset($_SESSION['flash_error'])) {
    $error = $_SESSION['flash_error'];
    unset($_SESSION['flash_error']);
}

// Ensure database array exists
if (!file_exists($db_file)) {
    file_put_contents($db_file, json_encode(["clients" => [], "admins" => []]));
}
$db = json_decode(file_get_contents($db_file), true);

// ------ Handle Login ------
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['login_admin'])) {
    $email = trim($_POST['admin_email']);
    $password = trim($_POST['admin_password']);
    
    if (isset($db['admins'][$email])) {
        if (isset($db['admins'][$email]['suspended']) && $db['admins'][$email]['suspended'] === true) {
            $error = 'Your admin account has been suspended. Contact superadmin.';
        } else if (password_verify($password, $db['admins'][$email]['password_hash'])) {
            $_SESSION['admin_logged_in'] = true;
            $_SESSION['admin_email'] = $email;
            $_SESSION['admin_name'] = $db['admins'][$email]['name'];
            header('Location: admin.php');
            exit;
        } else {
            $error = 'Invalid Password';
        }
    } else {
        $error = 'Admin account not found. Please contact Superadmin.';
    }
}

// If Logged In, handle actions and security
if (isset($_SESSION['admin_logged_in'])) {
    // Active Security: Force logout if admin is suspended or deleted
    require_once 'logger.php';
    $active_email = $_SESSION['admin_email'];
    if (!isset($db['admins'][$active_email]) || (isset($db['admins'][$active_email]['suspended']) && $db['admins'][$active_email]['suspended'] === true)) {
        unset($_SESSION['admin_logged_in']);
        unset($_SESSION['admin_email']);
        unset($_SESSION['admin_name']);
        $_SESSION['flash_error'] = 'Your sub-admin account has been suspended by the superadmin.';
        header('Location: admin.php');
        exit;
    }

    // Action: Delete a File
    if (isset($_GET['action']) && $_GET['action'] == 'delete_file' && isset($_GET['client']) && isset($_GET['file'])) {
        $client_email = $_GET['client'];
        $file_to_delete = basename($_GET['file']);
        
        if (isset($db['clients'][$client_email])) {
            $folder_id = $db['clients'][$client_email]['folder'];
            $filepath = $doc_dir . $folder_id . '/' . $file_to_delete;
            if (file_exists($filepath) && !is_dir($filepath)) {
                unlink($filepath);
                
                // Also remove from suspended_files array if it was suspended
                if (isset($db['clients'][$client_email]['suspended_files'])) {
                    $db['clients'][$client_email]['suspended_files'] = array_values(array_diff($db['clients'][$client_email]['suspended_files'], [$file_to_delete]));
                    file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                }

                $_SESSION['flash_success'] = "File '{$file_to_delete}' automatically removed from {$client_email}'s folder.";
                log_action('DELETE_FILE', 'Admin: ' . $_SESSION['admin_email'], 'Client: ' . $client_email, "Deleted file: {$file_to_delete}");
            }
        }
        header('Location: admin.php');
        exit;
    }

    // Action: Toggle Suspend Client
    if (isset($_GET['action']) && $_GET['action'] == 'toggle_suspend_client' && isset($_GET['client'])) {
        $client_email = $_GET['client'];
        if (isset($db['clients'][$client_email])) {
            $is_suspended = isset($db['clients'][$client_email]['suspended']) && $db['clients'][$client_email]['suspended'] === true;
            $db['clients'][$client_email]['suspended'] = !$is_suspended;
            file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
            $state = !$is_suspended ? 'suspended' : 'reactivated';
            $_SESSION['flash_success'] = "Client account for {$client_email} has been {$state}.";
            log_action('SUSPEND_CLIENT', 'Admin: ' . $_SESSION['admin_email'], 'Client: ' . $client_email, "Client suspension state toggled to: {$state}");
        }
        header('Location: admin.php');
        exit;
    }

    // Action: Toggle Suspend File
    if (isset($_GET['action']) && $_GET['action'] == 'toggle_suspend_file' && isset($_GET['client']) && isset($_GET['file'])) {
        $client_email = $_GET['client'];
        $file_to_suspend = basename($_GET['file']);
        if (isset($db['clients'][$client_email])) {
            if (!isset($db['clients'][$client_email]['suspended_files'])) {
                $db['clients'][$client_email]['suspended_files'] = [];
            }
            if (in_array($file_to_suspend, $db['clients'][$client_email]['suspended_files'])) {
                // Remove from suspension (unsuspend)
                $db['clients'][$client_email]['suspended_files'] = array_values(array_diff($db['clients'][$client_email]['suspended_files'], [$file_to_suspend]));
                $_SESSION['flash_success'] = "File '{$file_to_suspend}' is now visible to the client.";
            } else {
                // Add to suspension (suspend)
                $db['clients'][$client_email]['suspended_files'][] = $file_to_suspend;
                $_SESSION['flash_success'] = "File '{$file_to_suspend}' is now hidden from the client.";
            }
            file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
            log_action('SUSPEND_FILE', 'Admin: ' . $_SESSION['admin_email'], 'Client: ' . $client_email, "File visibility toggled for: {$file_to_suspend}");
        }
        header('Location: admin.php');
        exit;
    }

    // Action: Delete Client Entirely
    if (isset($_GET['action']) && $_GET['action'] == 'delete_client' && isset($_GET['client'])) {
        $client_email = $_GET['client'];
        if (isset($db['clients'][$client_email])) {
            unset($db['clients'][$client_email]);
            file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
            $_SESSION['flash_success'] = "Client account for {$client_email} completely deleted.";
            log_action('DELETE_CLIENT', 'Admin: ' . $_SESSION['admin_email'], 'Client: ' . $client_email, "Client deleted from database.");
        }
        header('Location: admin.php');
        exit;
    }

    // Action: Create Client
    if (isset($_POST['create_client'])) {
        $name = trim($_POST['name']);
        $email = trim($_POST['email']);
        $mobile = trim($_POST['mobile']);
        $password = trim($_POST['password']);
        
        $folder_id = strtolower(preg_replace('/[^a-zA-Z0-9]/', '_', $email));

        if (!empty($email) && !empty($password)) {
            if (isset($db['clients'][$email])) {
                 $_SESSION['flash_error'] = "A client with this email already exists.";
            } else {
                 $db['clients'][$email] = [
                     'name' => $name,
                     'mobile' => $mobile,
                     'password_hash' => password_hash($password, PASSWORD_DEFAULT),
                     'folder' => $folder_id,
                     'suspended' => false,
                     'suspended_files' => [],
                     'assigned_admin' => $_SESSION['admin_email']
                 ];
                 file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                 
                 $client_path = $doc_dir . $folder_id;
                 if (!is_dir($client_path)) {
                     mkdir($client_path, 0755, true);
                 }
                 $_SESSION['flash_success'] = "Client {$name} created successfully!";
                 log_action('CREATE_CLIENT', 'Admin: ' . $_SESSION['admin_email'], 'Client: ' . $email, "Provisioned new client vault.");
            }
        } else {
            $_SESSION['flash_error'] = "Email and Password are required.";
        }
        header('Location: admin.php');
        exit;
    }

    // Action: Upload Document
    if (isset($_POST['upload_doc'])) {
        $client_email = $_POST['client_email'];
        if (isset($db['clients'][$client_email]) && isset($_FILES['document'])) {
            $folder_id = $db['clients'][$client_email]['folder'];
            $target_dir = $doc_dir . $folder_id . '/';
            
            if (!is_dir($target_dir)) mkdir($target_dir, 0755, true);

            $file_name = basename($_FILES["document"]["name"]);
            $file_name = preg_replace("/[^a-zA-Z0-9\._-]/", "_", $file_name);
            $target_file = $target_dir . $file_name;

            if (move_uploaded_file($_FILES["document"]["tmp_name"], $target_file)) {
                $_SESSION['flash_success'] = "Document uploaded successfully to {$client_email}.";
                log_action('UPLOAD_FILE', 'Admin: ' . $_SESSION['admin_email'], 'Client: ' . $client_email, "Uploaded document: {$file_name}");
            } else {
                $php_error = $_FILES['document']['error'];
                if ($php_error !== UPLOAD_ERR_OK) {
                    $_SESSION['flash_error'] = "PHP Upload Error Code: " . $php_error . " (Check Plesk PHP upload_max_filesize limit)";
                } else if (!is_writable($target_dir)) {
                    $_SESSION['flash_error'] = "Permission Denied: Server cannot write to " . $target_dir . ". Check Plesk File Manager permissions.";
                } else {
                    $_SESSION['flash_error'] = "Failed to move uploaded file. Check folder ownership.";
                }
            }
        } else {
            $_SESSION['flash_error'] = "Please select a valid client and attach a file.";
        }
        header('Location: admin.php');
        exit;
    }
}
?>
<!doctype html>
<html>
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>Admin Dashboard - Balaji Electro Controls Pvt. Ltd.</title>
        <link rel="stylesheet" type="text/css" href="../content/public/css/bootstrap.min.css">
        <link rel="stylesheet" href="https://unpkg.com/aos@next/dist/aos.css">
        <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" integrity="sha384-fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr" crossorigin="anonymous" media="screen">
        <link rel="stylesheet" href="https://vadikom.github.io/smartmenus/src/css/sm-core-css.css">
        <link rel="stylesheet" type="text/css" href="../content/public/css/animate.css">
        <link rel="stylesheet" href="../content/public/css/sm-core-css.css">
        <link rel="stylesheet" type="text/css" href="../content/public/css/custom.css">
        <style>
            .admin-wrapper { padding: 50px 0; background: #eef2f5; }
            .card { border: none; box-shadow: 0 4px 15px rgba(0,0,0,0.05); margin-bottom: 30px; border-radius: 8px;}
            .card-header { background: white; border-bottom: 2px solid #eee; padding: 15px 25px; font-weight: bold; color: #252b65;}
            .card-body { padding: 25px; }

            /* Complete hard override for all inputs in the admin dashboard */
            .my-admin-input { 
                display: block;
                width: 100% !important; 
                max-width: 100% !important; 
                min-width: 0 !important;
                padding: 10px 12px !important; 
                border-radius: 4px !important; 
                border: 1px solid #ced4da !important; 
                box-sizing: border-box !important; 
                margin: 0 !important;
                background: #fff;
                font-family: inherit;
                font-size: 14px;
            }
            .my-admin-input:focus {
                border-color: #252b65 !important;
                outline: none !important;
                box-shadow: 0 0 5px rgba(37, 43, 101, 0.2);
            }

            .btn-brand { background: #252b65; color: white; padding: 10px 20px;}
            .btn-brand:hover { background: #1d224f; color: white; }
            .client-block { border: 1px solid #e0e0e0; border-radius: 8px; margin-bottom: 20px; overflow: hidden; background: #fafafa;}
            .client-header { background: #fff; padding: 15px 20px; border-bottom: 1px solid #eee; display: flex; justify-content: space-between; align-items: center;}
            .client-header h5 { margin: 0; color: #333; font-weight: bold;}
            .badge-folder { background: #e9ecef; color: #495057; padding: 4px 8px; border-radius: 4px; font-family: monospace; font-size: 12px;}
            .doc-list { list-style: none; padding: 0; margin: 0; }
            .doc-list li { padding: 12px 15px; border-bottom: 1px solid #f1f1f1; display: flex; justify-content: space-between; align-items: center;}
            .logout-admin { background: #dc3545; color: white; padding: 8px 15px; border-radius: 4px; text-decoration: none;}
            .logout-admin:hover { background: #c82333; color: white; text-decoration: none;}
            .suspended-badge { background: #ffc107; color: #000; padding: 3px 8px; font-size: 11px; border-radius: 4px; text-transform: uppercase; font-weight: bold; margin-left: 10px; }
            .file-suspended { opacity: 0.5; }
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
                                    <h2 style="font-size:24px;"><b>ADMINISTRATOR PORTAL</b></h2>
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
                                <li class="breadcrumb-item active" aria-current="page">Admin Management</li>
                            </ol>
                        </nav>
                    </div>
                </div>
            </div>
        </div>

        <!-- MAIN DASHBOARD CONTENT -->
        <div class="admin-wrapper">
            <div class="container">
                <div class="row mb-4">
                    <div class="col-12 d-flex justify-content-between align-items-center">
                        <h2 style="color:#252b65; font-weight:bold;">Admin Portal Dashboard</h2>
                        <?php if(isset($_SESSION['admin_logged_in'])): ?>
                            <a href="?logout=1" class="logout-admin"><i class="fas fa-power-off"></i> Logout Admin</a>
                        <?php endif; ?>
                    </div>
                </div>

                <?php if ($error): ?>
                    <div class="alert alert-danger shadow-sm"><i class="fas fa-exclamation-triangle"></i> <?php echo $error; ?></div>
                <?php endif; ?>
                <?php if ($success): ?>
                    <div class="alert alert-success shadow-sm"><i class="fas fa-check-circle"></i> <?php echo $success; ?></div>
                <?php endif; ?>

                <?php if (!isset($_SESSION['admin_logged_in'])): ?>
                
                    <div class="row justify-content-center mt-5">
                        <div class="col-md-5">
                            <div class="card">
                                <div class="card-header text-center py-4">
                                    <i class="fas fa-lock fa-3x mb-3" style="color:#ccc;"></i>
                                    <h4>Admin Access Required</h4>
                                </div>
                                <div class="card-body">
                                    <form method="POST">
                                        <div class="form-group mb-3" style="overflow: hidden;">
                                            <label class="text-muted">Admin Email</label>
                                            <input type="email" name="admin_email" class="my-admin-input" required placeholder="admin@bec.com">
                                        </div>
                                        <div class="form-group mb-4" style="overflow: hidden;">
                                            <label class="text-muted">Password</label>
                                            <input type="password" name="admin_password" class="my-admin-input" required placeholder="Enter password...">
                                        </div>
                                        <button type="submit" name="login_admin" class="btn btn-brand w-100"><i class="fas fa-lock-open"></i> Secure Login</button>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>

                <?php else: ?>
                    
                    <div class="row">
                        <!-- Left Column: Operations -->
                        <div class="col-lg-4">
                            
                            <!-- Create Client Panel (Moved to Top) -->
                            <div class="card">
                                <div class="card-header"><i class="fas fa-user-plus text-primary"></i> Register New Client</div>
                                <div class="card-body">
                                    <form method="POST">
                                        <div class="form-group mb-3">
                                            <label class="text-sm font-weight-bold">Contact Name / Company</label>
                                            <input type="text" name="name" class="my-admin-input" placeholder="e.g. Acme Corp" required>
                                        </div>
                                        <div class="form-group mb-3">
                                            <label class="text-sm font-weight-bold">Login Email</label>
                                            <input type="email" name="email" class="my-admin-input" placeholder="email@company.com" required>
                                        </div>
                                        <div class="form-group mb-3">
                                            <label class="text-sm font-weight-bold">Mobile</label>
                                            <input type="tel" name="mobile" class="my-admin-input" required>
                                        </div>
                                        <div class="form-group mb-4">
                                            <label class="text-sm font-weight-bold">Client Password</label>
                                            <input type="text" name="password" class="my-admin-input" placeholder="SecurePass123!" required>
                                        </div>
                                        <button type="submit" name="create_client" class="btn btn-success w-100"><i class="fas fa-save"></i> Create & Setup Vault</button>
                                    </form>
                                </div>
                            </div>

                            <!-- Upload Panel (Moved Below Create) -->
                            <div class="card">
                                <div class="card-header"><i class="fas fa-cloud-upload-alt text-primary"></i> Upload Document</div>
                                <div class="card-body">
                                    <form method="POST" enctype="multipart/form-data">
                                        <div class="form-group mb-3">
                                            <label class="text-sm font-weight-bold">Assign to Client</label>
                                            <select name="client_email" class="my-admin-input" required>
                                                <option value="">-- Select Client --</option>
                                                <?php foreach($db['clients'] as $email => $clientData): 
                                                    if(isset($clientData['assigned_admin']) && $clientData['assigned_admin'] !== $_SESSION['admin_email']) continue;
                                                ?>
                                                    <option value="<?php echo htmlspecialchars($email); ?>">
                                                        <?php echo htmlspecialchars($clientData['name']); ?>
                                                    </option>
                                                <?php endforeach; ?>
                                            </select>
                                        </div>
                                        <div class="form-group mb-4">
                                            <label class="text-sm font-weight-bold">File (PDF, Doc)</label>
                                            <input type="file" name="document" class="my-admin-input" style="padding: 6px !important;" required>
                                        </div>
                                        <button type="submit" name="upload_doc" class="btn btn-brand w-100"><i class="fas fa-upload"></i> Upload Securely</button>
                                    </form>
                                </div>
                            </div>
                        </div>

                        <!-- Right Column: Roster & Details -->
                        <div class="col-lg-8">
                            <div class="card">
                                <div class="card-header d-flex justify-content-between align-items-center">
                                    <span><i class="fas fa-users text-primary"></i> Full Client Roster</span>
                                    <span class="badge badge-primary bg-primary"><?php echo count($db['clients']); ?> Active</span>
                                </div>
                                <div class="card-body p-4">
                                    
                                    <?php 
                                        $admin_clients = 0;
                                        foreach($db['clients'] as $c) {
                                            if(!isset($c['assigned_admin']) || $c['assigned_admin'] == $_SESSION['admin_email']) $admin_clients++;
                                        }
                                    ?>
                                        <?php if ($admin_clients == 0): ?>
                                        <div class="text-center text-muted py-5">
                                            <i class="fas fa-folder-open fa-3x mb-3 text-light"></i>
                                            <h5>No Clients Yet</h5>
                                            <p>Use the panel on the left to register a new client under your command.</p>
                                        </div>
                                    <?php else: ?>

                                        <?php foreach($db['clients'] as $email => $clientData): 
                                            // Only show clients owned by THIS admin
                                            if(isset($clientData['assigned_admin']) && $clientData['assigned_admin'] !== $_SESSION['admin_email']) continue;
                                            
                                            // Fetch files for this client
                                            $client_files = [];
                                            $client_path = $doc_dir . $clientData['folder'];
                                            if (is_dir($client_path)) {
                                                $scanned = array_diff(scandir($client_path), array('..', '.'));
                                                foreach ($scanned as $item) {
                                                    if (!is_dir($client_path . '/' . $item)) {
                                                        $client_files[] = $item;
                                                    }
                                                }
                                            }
                                            $is_suspended = isset($clientData['suspended']) && $clientData['suspended'] === true;
                                            $suspended_files = isset($clientData['suspended_files']) ? $clientData['suspended_files'] : [];
                                        ?>
                                            <div class="client-block">
                                                <div class="client-header" style="<?php if($is_suspended) echo 'background:#ffeeba; border-color:#ffdf7e;'; ?>">
                                                    <div>
                                                        <h5 style="color:#252b65;">
                                                            <?php echo htmlspecialchars($clientData['name']); ?>
                                                            <?php if($is_suspended): ?><span class="suspended-badge"><i class="fas fa-ban"></i> Suspended</span><?php endif; ?>
                                                        </h5>
                                                        <div class="text-sm mt-1" style="color:#666;">
                                                            <i class="fas fa-envelope"></i> <?php echo htmlspecialchars($email); ?> &nbsp;|&nbsp;  
                                                            <i class="fas fa-folder"></i> <span class="badge-folder"><?php echo htmlspecialchars($clientData['folder']); ?></span>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <a href="?action=toggle_suspend_client&client=<?php echo urlencode($email); ?>" class="btn btn-sm btn-<?php echo $is_suspended ? 'success' : 'warning'; ?>" title="Suspend / Unsuspend">
                                                            <i class="fas fa-<?php echo $is_suspended ? 'check' : 'pause'; ?>"></i>
                                                        </a>
                                                        <a href="?action=delete_client&client=<?php echo urlencode($email); ?>" onclick="return confirm('Are you sure you want to permanently delete this client login? (Files will remain in folder)');" class="btn btn-sm btn-outline-danger" title="Delete Client"><i class="fas fa-trash"></i></a>
                                                    </div>
                                                </div>
                                                <div class="client-content p-3">
                                                    <?php if (count($client_files) > 0): ?>
                                                        <ul class="doc-list">
                                                            <?php foreach ($client_files as $file): 
                                                                $file_is_suspended = in_array($file, $suspended_files);
                                                            ?>
                                                                <li class="<?php if($file_is_suspended) echo 'file-suspended'; ?>">
                                                                    <span>
                                                                        <i class="fas fa-file-pdf text-danger"></i> &nbsp; 
                                                                        <strong style="color:#444;"><?php echo htmlspecialchars($file); ?></strong>
                                                                        <?php if($file_is_suspended): ?> <span class="badge badge-warning bg-warning text-dark ml-2">Hidden</span> <?php endif; ?>
                                                                    </span>
                                                                    <div>
                                                                        <a href="?action=toggle_suspend_file&client=<?php echo urlencode($email); ?>&file=<?php echo urlencode($file); ?>" class="btn btn-sm text-<?php echo $file_is_suspended ? 'success' : 'warning'; ?>" style="box-shadow:none;"><i class="fas fa-<?php echo $file_is_suspended ? 'eye' : 'eye-slash'; ?>"></i> <?php echo $file_is_suspended ? 'Show' : 'Hide'; ?></a>
                                                                        <a href="?action=delete_file&client=<?php echo urlencode($email); ?>&file=<?php echo urlencode($file); ?>" onclick="return confirm('Immediately delete this file permanently?');" class="btn btn-sm text-danger" style="box-shadow:none;"><i class="fas fa-times-circle"></i> Delete</a>
                                                                    </div>
                                                                </li>
                                                            <?php endforeach; ?>
                                                        </ul>
                                                    <?php else: ?>
                                                        <span class="text-muted text-sm d-block text-center p-3" style="background:#f9f9f9; border-radius:4px;"><i class="fas fa-info-circle"></i> No documents uploaded yet.</span>
                                                    <?php endif; ?>
                                                </div>
                                            </div>
                                        <?php endforeach; ?>
                                    
                                    <?php endif; ?>
                                </div>
                            </div>
                        </div>
                    </div>
                <?php endif; ?>
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
