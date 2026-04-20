<?php
session_start();

// ====== Configuration ======
define('SUPERADMIN_PASSWORD', 'BalajiSuper2026'); // CHANGE THIS IN PRODUCTION
$db_file = 'database.json';
$doc_dir = 'client_documents/';

if (!is_dir($doc_dir)) {
    mkdir($doc_dir, 0755, true);
}

// Ensure database array exists
if (!file_exists($db_file)) {
    file_put_contents($db_file, json_encode(["clients" => [], "admins" => []]));
}
$db = json_decode(file_get_contents($db_file), true);
if(!isset($db['admins'])) {
    $db['admins'] = [];
    file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
}

// ------ Handle Logout ------
if (isset($_GET['logout'])) {
    unset($_SESSION['superadmin_logged_in']);
    header('Location: superadmin.php');
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

// ------ Handle Login ------
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['login_superadmin'])) {
    if ($_POST['super_password'] === SUPERADMIN_PASSWORD) {
        $_SESSION['superadmin_logged_in'] = true;
        header('Location: superadmin.php');
        exit;
    } else {
        $error = 'Invalid Superadmin Password';
    }
}


// If Logged In, handle actions
if (isset($_SESSION['superadmin_logged_in'])) {
    require_once 'logger.php';

    // ADMIN ACTIONS
    if (isset($_GET['action']) && $_GET['action'] == 'toggle_suspend_admin' && isset($_GET['admin_email'])) {
        $admin_email = $_GET['admin_email'];
        if (isset($db['admins'][$admin_email])) {
            $is_suspended = isset($db['admins'][$admin_email]['suspended']) && $db['admins'][$admin_email]['suspended'] === true;
            $db['admins'][$admin_email]['suspended'] = !$is_suspended;
            file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
            $state = !$is_suspended ? 'suspended' : 'reactivated';
            $_SESSION['flash_success'] = "Sub-Admin {$admin_email} has been {$state}.";
            log_action('SUSPEND_ADMIN', 'Superadmin', 'Admin: ' . $admin_email, "Admin suspension toggled to: {$state}");
        }
        header('Location: superadmin.php');
        exit;
    }

    if (isset($_GET['action']) && $_GET['action'] == 'delete_admin' && isset($_GET['admin_email'])) {
        $admin_email = $_GET['admin_email'];
        if (isset($db['admins'][$admin_email])) {
            unset($db['admins'][$admin_email]);
            file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
            $_SESSION['flash_success'] = "Admin account for {$admin_email} has been permanently deleted.";
            log_action('DELETE_ADMIN', 'Superadmin', 'Admin: ' . $admin_email, "Sub-admin was permanently deleted.");
        }
        header('Location: superadmin.php');
        exit;
    }

    if (isset($_POST['create_admin'])) {
        $name = trim($_POST['name']);
        $email = trim($_POST['email']);
        $password = trim($_POST['password']);
        
        if (!empty($email) && !empty($password)) {
            if (isset($db['admins'][$email])) {
                 $_SESSION['flash_error'] = "An admin with this email already exists.";
            } else {
                 $db['admins'][$email] = [
                     'name' => $name,
                     'password_hash' => password_hash($password, PASSWORD_DEFAULT),
                     'created_at' => date("Y-m-d H:i:s")
                 ];
                 file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                 $_SESSION['flash_success'] = "Sub-Admin {$name} created successfully!";
                 log_action('CREATE_ADMIN', 'Superadmin', 'Admin: ' . $email, "Provisioned new sub-admin.");
            }
        }
        header('Location: superadmin.php');
        exit;
    }

    // CLIENT ACTIONS
    if (isset($_POST['create_client'])) {
        $name = trim($_POST['name']);
        $email = trim($_POST['email']);
        $mobile = trim($_POST['mobile']);
        $password = trim($_POST['password']);
        $assigned_admin = trim($_POST['assigned_admin']); // Can be empty if unassigned/superadmin owned
        
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
                     'assigned_admin' => $assigned_admin
                 ];
                 file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                 
                 $client_path = $doc_dir . $folder_id;
                 if (!is_dir($client_path)) mkdir($client_path, 0755, true);
                 
                 $_SESSION['flash_success'] = "Client {$name} created successfully!";
                 log_action('CREATE_CLIENT', 'Superadmin', 'Client: ' . $email, "Created global client (Assigned: {$assigned_admin}).");
            }
        }
        header('Location: superadmin.php');
        exit;
    }

    if (isset($_POST['transfer_client'])) {
        $client_email = $_POST['client_email'];
        $new_admin = $_POST['new_admin'];
        if (isset($db['clients'][$client_email])) {
            $db['clients'][$client_email]['assigned_admin'] = $new_admin;
            file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
            $_SESSION['flash_success'] = "Client {$client_email} successfully transferred.";
            log_action('TRANSFER_CLIENT', 'Superadmin', 'Client: ' . $client_email, "Transferred ownership to: " . ($new_admin ? $new_admin : 'Superadmin'));
        }
        header('Location: superadmin.php');
        exit;
    }

    if (isset($_POST['upload_doc'])) {
        $client_email = $_POST['client_email'];
        if (isset($db['clients'][$client_email]) && isset($_FILES['document'])) {
            $folder_id = $db['clients'][$client_email]['folder'];
            $target_dir = $doc_dir . $folder_id . '/';
            
            $project_folder = trim($_POST['project_folder'] ?? '');
            if (!empty($project_folder)) {
                $project_folder = str_replace(['/', '\\', '..', '<', '>', '|', ':', '*', '?', '"'], '', $project_folder);
                $target_dir .= $project_folder . '/';
            }

            if (!is_dir($target_dir)) mkdir($target_dir, 0755, true);
            $file_name = str_replace(['/', '\\', '..'], '_', basename($_FILES["document"]["name"]));
            $target_file = $target_dir . $file_name;
            
            if (move_uploaded_file($_FILES["document"]["tmp_name"], $target_file)) {
                $_SESSION['flash_success'] = "Document uploaded successfully to {$client_email}.";
                log_action('UPLOAD_FILE', 'Superadmin', 'Client: ' . $client_email, "Uploaded document: {$file_name}");
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
        }
        header('Location: superadmin.php');
        exit;
    }

    if (isset($_GET['action'])) {
        // Toggle Suspend Client
        if ($_GET['action'] == 'toggle_suspend_client' && isset($_GET['client'])) {
            $client = $_GET['client'];
            if (isset($db['clients'][$client])) {
                $db['clients'][$client]['suspended'] = !($db['clients'][$client]['suspended'] ?? false);
                file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                $_SESSION['flash_success'] = "Client {$client} suspension toggled.";
                log_action('SUSPEND_CLIENT', 'Superadmin', 'Client: ' . $client, "Client suspension state toggled globally.");
            }
            header('Location: superadmin.php'); exit;
        }

        // Delete Client
        if ($_GET['action'] == 'delete_client' && isset($_GET['client'])) {
            $client = $_GET['client'];
            if (isset($db['clients'][$client])) {
                unset($db['clients'][$client]);
                file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                $_SESSION['flash_success'] = "Client {$client} deleted forever.";
                log_action('DELETE_CLIENT', 'Superadmin', 'Client: ' . $client, "Client deleted from system.");
            }
            header('Location: superadmin.php'); exit;
        }

        // Toggle Suspend File
        if ($_GET['action'] == 'toggle_suspend_file' && isset($_GET['client']) && isset($_GET['file'])) {
            $client = $_GET['client'];
            $file = preg_replace('/(\.\.\/|\.\.\\\\)/', '', $_GET['file']);
            if (isset($db['clients'][$client])) {
                if(!isset($db['clients'][$client]['suspended_files'])) $db['clients'][$client]['suspended_files'] = [];
                if(in_array($file, $db['clients'][$client]['suspended_files'])) {
                    $db['clients'][$client]['suspended_files'] = array_values(array_diff($db['clients'][$client]['suspended_files'], [$file]));
                } else {
                    $db['clients'][$client]['suspended_files'][] = $file;
                }
                file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                $_SESSION['flash_success'] = "File visibility toggled.";
                log_action('SUSPEND_FILE', 'Superadmin', 'Client: ' . $client, "File visibility toggled globally.");
            }
            header('Location: superadmin.php'); exit;
        }

        // Delete file
        if ($_GET['action'] == 'delete_file' && isset($_GET['client']) && isset($_GET['file'])) {
            $client = $_GET['client'];
            $file = preg_replace('/(\.\.\/|\.\.\\\\)/', '', $_GET['file']);
            if (isset($db['clients'][$client])) {
                $filepath = $doc_dir . $db['clients'][$client]['folder'] . '/' . $file;
                if(file_exists($filepath) && !is_dir($filepath)) {
                    unlink($filepath);
                    if (isset($db['clients'][$client]['suspended_files'])) {
                        $db['clients'][$client]['suspended_files'] = array_values(array_diff($db['clients'][$client]['suspended_files'], [$file]));
                        file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                    }
                    $_SESSION['flash_success'] = "File {$file} deleted.";
                    log_action('DELETE_FILE', 'Superadmin', 'Client: ' . $client, "Deleted file globally: {$file}");
                }
            }
            header('Location: superadmin.php'); exit;
        }

        // Delete folder
        if ($_GET['action'] == 'delete_folder' && isset($_GET['client']) && isset($_GET['folder'])) {
            $client = $_GET['client'];
            $folder = preg_replace('/(\.\.\/|\.\.\\\\)/', '', $_GET['folder']);
            if (isset($db['clients'][$client])) {
                $folderpath = $doc_dir . $db['clients'][$client]['folder'] . '/' . $folder;
                if (is_dir($folderpath)) {
                    $fp = opendir($folderpath);
                    while (false !== ($f = readdir($fp))) {
                        if ($f == '.' || $f == '..') continue;
                        if (!is_dir($folderpath . '/' . $f)) {
                            unlink($folderpath . '/' . $f);
                            $rel_path = $folder . '/' . $f;
                            if (isset($db['clients'][$client]['suspended_files'])) {
                                $db['clients'][$client]['suspended_files'] = array_values(array_diff($db['clients'][$client]['suspended_files'], [$rel_path]));
                            }
                        }
                    }
                    closedir($fp);
                    rmdir($folderpath);
                    
                    if (isset($db['clients'][$client]['suspended_folders'])) {
                        $db['clients'][$client]['suspended_folders'] = array_values(array_diff($db['clients'][$client]['suspended_folders'], [$folder]));
                    }
                    file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                    $_SESSION['flash_success'] = "Folder {$folder} deleted.";
                    log_action('DELETE_FOLDER', 'Superadmin', 'Client: ' . $client, "Deleted folder globally: {$folder}");
                }
            }
            header('Location: superadmin.php'); exit;
        }

        // Toggle Suspend Folder
        if ($_GET['action'] == 'toggle_suspend_folder' && isset($_GET['client']) && isset($_GET['folder'])) {
            $client = $_GET['client'];
            $folder = preg_replace('/(\.\.\/|\.\.\\\\)/', '', $_GET['folder']);
            if (isset($db['clients'][$client])) {
                if(!isset($db['clients'][$client]['suspended_folders'])) $db['clients'][$client]['suspended_folders'] = [];
                if(in_array($folder, $db['clients'][$client]['suspended_folders'])) {
                    $db['clients'][$client]['suspended_folders'] = array_values(array_diff($db['clients'][$client]['suspended_folders'], [$folder]));
                    $_SESSION['flash_success'] = "Folder visibility toggled (Visible).";
                } else {
                    $db['clients'][$client]['suspended_folders'][] = $folder;
                    $_SESSION['flash_success'] = "Folder visibility toggled (Hidden).";
                }
                file_put_contents($db_file, json_encode($db, JSON_PRETTY_PRINT));
                log_action('SUSPEND_FOLDER', 'Superadmin', 'Client: ' . $client, "Folder visibility toggled.");
            }
            header('Location: superadmin.php'); exit;
        }
    }
}
?>
<!doctype html>
<html>
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>Superadmin - Balaji Electro Controls Pvt. Ltd.</title>
        <link rel="stylesheet" type="text/css" href="../content/public/css/bootstrap.min.css">
        <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" media="screen">
        <style>
            body { background: #1a1e4a; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; overflow-x: hidden; }
            .superadmin-wrapper { padding: 40px 0; max-width: 1600px; margin: 0 auto;}
            .card { border: none; box-shadow: 0 10px 30px rgba(0,0,0,0.3); border-radius: 12px; margin-bottom: 30px;}
            .card-header { background: #fff; border-bottom: 2px solid #f1f1f1; padding: 20px 25px; font-weight: bold; color: #1a1e4a; font-size: 1.1rem; border-radius: 12px 12px 0 0;}
            .card-body { padding: 30px; background: #fff; border-radius: 0 0 12px 12px;}
            
            .super-input { 
                display: block; width: 100%; padding: 10px 15px; border-radius: 6px; 
                border: 2px solid #e1e4e8; background: #f8f9fa; font-size: 14px; transition: 0.3s;
                margin-bottom: 15px; box-sizing: border-box;
            }
            .super-input:focus { border-color: #ff3366; outline: none; background: #fff; }
            
            .btn-super { background: #ff3366; color: white; padding: 10px 20px; font-weight: bold; text-transform: uppercase; letter-spacing: 1px; border: none; border-radius: 6px; transition: 0.3s;}
            .btn-super:hover { background: #e62050; color: white; transform: translateY(-2px); box-shadow: 0 5px 15px rgba(255, 51, 102, 0.4);}
            
            .header-banner { text-align: center; color: white; margin-bottom: 30px; }
            .header-banner i { font-size: 40px; color: #ff3366; margin-bottom: 10px; }
            
            .admin-list { list-style: none; padding: 0; margin: 0; }
            .admin-list li { display: flex; justify-content: space-between; align-items: center; padding: 15px; border-bottom: 1px solid #f1f1f1; }
            .admin-list li:last-child { border-bottom: none; }
            
            .logout-super { position: absolute; top: 20px; right: 30px; color: rgba(255,255,255,0.6); text-decoration: none; font-weight: bold;}
            .logout-super:hover { color: white; text-decoration: none; }
            
            .audit-super { position: absolute; top: 20px; right: 130px; background: rgba(255,255,255,0.1); color: white; text-decoration: none; padding: 5px 15px; border-radius: 20px; font-weight: bold; font-size: 13px; transition: 0.3s;}
            .audit-super:hover { background: rgba(255,255,255,0.25); color: white; text-decoration: none;}

            .stat-box { background: rgba(255,255,255,0.1); border-radius: 8px; padding: 20px; text-align: center; color: white; margin-bottom:30px; }
            .stat-box h3 { margin: 0; font-size: 30px; font-weight: border; color: #ff3366;}
            .stat-box p { margin: 0; text-transform: uppercase; font-size: 11px; letter-spacing: 1px; opacity: 0.8;}

            .text-sm { font-size: 12px; }
            
            .client-block { border: 1px solid #e0e0e0; border-radius: 8px; margin-bottom: 20px; overflow: hidden; background: #fafafa;}
            .client-header { background: #fff; padding: 15px 20px; border-bottom: 1px solid #eee; display: flex; justify-content: space-between; align-items: center;}
            .client-header h5 { margin: 0; color: #333; font-weight: bold;}
            .badge-folder { background: #e9ecef; color: #495057; padding: 4px 8px; border-radius: 4px; font-family: monospace; font-size: 12px;}
            .doc-list { list-style: none; padding: 0; margin: 0; }
            .doc-list li { padding: 12px 15px; border-bottom: 1px solid #f1f1f1; display: flex; justify-content: space-between; align-items: center;}
            
            .suspended-badge { background: #ffc107; color: #000; padding: 3px 8px; font-size: 11px; border-radius: 4px; text-transform: uppercase; font-weight: bold; margin-left: 10px; }
            .file-suspended { opacity: 0.5; }
        </style>
    </head>
    <body>
        
        <?php if(isset($_SESSION['superadmin_logged_in'])): ?>
            <a href="audit_reports.php" class="audit-super"><i class="fas fa-file-excel"></i> Audit Logs</a>
            <a href="?logout=1" class="logout-super"><i class="fas fa-sign-out-alt"></i> Logout</a>
        <?php endif; ?>

        <div class="superadmin-wrapper px-4">
            
            <div class="header-banner">
                <i class="fas fa-shield-alt"></i>
                <h2 style="font-weight: 800; letter-spacing: 2px;">SUPERADMIN CORE</h2>
                <p style="opacity: 0.7;">Total System Control</p>
            </div>

            <?php if ($error): ?>
                <div class="alert alert-danger shadow-sm mx-auto" style="max-width:800px;"><i class="fas fa-exclamation-triangle"></i> <?php echo $error; ?></div>
            <?php endif; ?>
            <?php if ($success): ?>
                <div class="alert alert-success shadow-sm mx-auto" style="max-width:800px;"><i class="fas fa-check-circle"></i> <?php echo $success; ?></div>
            <?php endif; ?>

            <?php if (!isset($_SESSION['superadmin_logged_in'])): ?>
            
                <div class="row justify-content-center mt-5">
                    <div class="col-md-5">
                        <div class="card">
                            <div class="card-header text-center py-4" style="border-bottom: none;">
                                <h4 style="margin:0;">God Mode Access</h4>
                            </div>
                            <div class="card-body">
                                <form method="POST">
                                    <input type="password" name="super_password" class="super-input" required placeholder="Enter Superadmin Core Password...">
                                    <button type="submit" name="login_superadmin" class="btn btn-super w-100"><i class="fas fa-fingerprint"></i> Authenticate</button>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>

            <?php else: ?>
                
                <!-- Global Stats -->
                <div class="row">
                    <div class="col-md-3">
                        <div class="stat-box">
                            <h3><?php echo count($db['admins']); ?></h3>
                            <p>Active Sub-Admins</p>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="stat-box">
                            <h3><?php echo count($db['clients']); ?></h3>
                            <p>Total Client Vaults</p>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="stat-box">
                            <?php 
                                $total_files = 0;
                                foreach($db['clients'] as $c) {
                                    $path = 'client_documents/' . $c['folder'];
                                    if(is_dir($path)) $total_files += count(array_diff(scandir($path), array('..', '.')));
                                }
                            ?>
                            <h3><?php echo $total_files; ?></h3>
                            <p>Total Secure Files</p>
                        </div>
                    </div>
                    <div class="col-md-3">
                         <div class="stat-box">
                            <?php 
                                $susp = 0;
                                foreach($db['clients'] as $c) { if(isset($c['suspended']) && $c['suspended']) $susp++; }
                            ?>
                            <h3 style="color:#ffc107;"><?php echo $susp; ?></h3>
                            <p>Suspended Clients</p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <!-- Column 1: Sub Admin MGMT -->
                    <div class="col-lg-3">
                        <div class="card">
                            <div class="card-header"><i class="fas fa-user-shield text-danger"></i> Provision Sub-Admin</div>
                            <div class="card-body">
                                <form method="POST">
                                    <label class="text-sm font-weight-bold" style="color:#666;">Admin Name</label>
                                    <input type="text" name="name" class="super-input" required>
                                    
                                    <label class="text-sm font-weight-bold" style="color:#666;">Login Email</label>
                                    <input type="email" name="email" class="super-input" required>
                                    
                                    <label class="text-sm font-weight-bold" style="color:#666;">Temporary Password</label>
                                    <input type="text" name="password" class="super-input" required>
                                    
                                    <button type="submit" name="create_admin" class="btn btn-super w-100"><i class="fas fa-plus-circle"></i> Grant Access</button>
                                </form>
                            </div>
                        </div>

                        <div class="card">
                            <div class="card-header"><i class="fas fa-users-cog text-primary"></i> Sub-Admin Network</div>
                            <div class="card-body p-0">
                                <?php if (count($db['admins']) == 0): ?>
                                    <div class="text-center text-muted py-4">
                                        <p class="mb-0">No Sub-Admins Provisioned</p>
                                    </div>
                                <?php else: ?>
                                    <ul class="admin-list">
                                        <?php foreach($db['admins'] as $email => $adminData): 
                                            $admin_suspended = isset($adminData['suspended']) && $adminData['suspended'] === true;
                                        ?>
                                            <li style="flex-direction:column; align-items:flex-start; <?php if($admin_suspended) echo 'background:#ffeeba; border-color:#ffdf7e;'; ?>">
                                                <div style="width:100%; display:flex; justify-content:space-between; align-items:center;">
                                                    <strong style="color:#1a1e4a; font-size: 1rem;">
                                                        <?php echo htmlspecialchars($adminData['name']); ?>
                                                        <?php if($admin_suspended): ?><span class="suspended-badge text-dark bg-warning" style="padding:2px 6px; font-size:10px; border-radius:3px;">Suspended</span><?php endif; ?>
                                                    </strong>
                                                    <div>
                                                        <a href="?action=toggle_suspend_admin&admin_email=<?php echo urlencode($email); ?>" class="btn btn-sm btn-<?php echo $admin_suspended ? 'success' : 'warning'; ?> py-0 px-2" title="Toggle Access"><i class="fas fa-<?php echo $admin_suspended ? 'check' : 'pause'; ?>"></i></a>
                                                        <a href="?action=delete_admin&admin_email=<?php echo urlencode($email); ?>" onclick="return confirm('REVOKE ACCESS?');" class="btn btn-sm btn-outline-danger py-0 px-2" title="Delete Admin"><i class="fas fa-trash"></i></a>
                                                    </div>
                                                </div>
                                                <div class="text-muted mt-1" style="font-size: 0.8rem;">
                                                    <i class="fas fa-envelope"></i> <?php echo htmlspecialchars($email); ?>
                                                </div>
                                            </li>
                                        <?php endforeach; ?>
                                    </ul>
                                <?php endif; ?>
                            </div>
                        </div>
                    </div>

                    <!-- Column 2: Client Controls -->
                    <div class="col-lg-3">
                         <!-- Create Client Panel -->
                         <div class="card">
                            <div class="card-header"><i class="fas fa-user-plus text-primary"></i> Create Global Client</div>
                            <div class="card-body">
                                <form method="POST">
                                    <div class="form-group mb-2">
                                        <label class="text-sm font-weight-bold" style="color:#666;">Company Name</label>
                                        <input type="text" name="name" class="super-input" required>
                                    </div>
                                    <div class="form-group mb-2">
                                        <label class="text-sm font-weight-bold" style="color:#666;">Login Email</label>
                                        <input type="email" name="email" class="super-input" required>
                                    </div>
                                    <div class="form-group mb-2">
                                        <label class="text-sm font-weight-bold" style="color:#666;">Mobile</label>
                                        <input type="tel" name="mobile" class="super-input" required>
                                    </div>
                                    <div class="form-group mb-2">
                                        <label class="text-sm font-weight-bold" style="color:#666;">Client Password</label>
                                        <input type="text" name="password" class="super-input" required>
                                    </div>
                                    <div class="form-group mb-3">
                                        <label class="text-sm font-weight-bold" style="color:#666;">Assign to Admin</label>
                                        <select name="assigned_admin" class="super-input">
                                            <option value="">-- No Admin (Superadmin Only) --</option>
                                            <?php foreach($db['admins'] as $e => $a): ?>
                                                <option value="<?php echo htmlspecialchars($e); ?>"><?php echo htmlspecialchars($a['name'] . " ($e)"); ?></option>
                                            <?php endforeach; ?>
                                        </select>
                                    </div>
                                    <button type="submit" name="create_client" class="btn btn-super w-100" style="background:#28a745;"><i class="fas fa-save"></i> Create Client</button>
                                </form>
                            </div>
                        </div>

                        <!-- Upload Panel -->
                        <div class="card">
                            <div class="card-header"><i class="fas fa-cloud-upload-alt text-primary"></i> Global Upload</div>
                            <div class="card-body">
                                <form method="POST" enctype="multipart/form-data">
                                    <div class="form-group mb-2">
                                        <label class="text-sm font-weight-bold" style="color:#666;">Target Client</label>
                                        <select id="uploadClientSelect" name="client_email" class="super-input" required onchange="updateFoldersDatalist()">
                                            <option value="">-- Select Client --</option>
                                            <?php foreach($db['clients'] as $email => $clientData): ?>
                                                <option value="<?php echo htmlspecialchars($email); ?>">
                                                    <?php echo htmlspecialchars($clientData['name']); ?>
                                                </option>
                                            <?php endforeach; ?>
                                        </select>
                                    </div>
                                    <div class="form-group mb-2">
                                        <label class="text-sm font-weight-bold" style="color:#666;">Project Name / Folder</label>
                                        <input type="text" name="project_folder" list="folderOptions" autocomplete="off" class="super-input" placeholder="Optional: Select or Type New...">
                                        <datalist id="folderOptions"></datalist>
                                    </div>
                                    <script>
                                        const clientFoldersMap = {
                                            <?php foreach($db['clients'] as $email => $clientData): 
                                                $c_path = $doc_dir . $clientData['folder'];
                                                $f_arr = [];
                                                if(is_dir($c_path)){
                                                    $sc = array_diff(scandir($c_path), ['.','..']);
                                                    foreach($sc as $i){
                                                        if(is_dir($c_path.'/'.$i)) $f_arr[] = addslashes($i);
                                                    }
                                                }
                                                echo "'" . addslashes($email) . "': [" . implode(',', array_map(function($f){return "'".$f."'";}, $f_arr)) . "],\n";
                                            endforeach; ?>
                                        };
                                        function updateFoldersDatalist() {
                                            const clientEmail = document.getElementById('uploadClientSelect').value;
                                            const datalist = document.getElementById('folderOptions');
                                            datalist.innerHTML = '';
                                            if (clientEmail && clientFoldersMap[clientEmail]) {
                                                clientFoldersMap[clientEmail].forEach(folder => {
                                                    let opt = document.createElement('option');
                                                    opt.value = folder;
                                                    datalist.appendChild(opt);
                                                });
                                            }
                                        }
                                    </script>
                                    <div class="form-group mb-3">
                                        <label class="text-sm font-weight-bold" style="color:#666;">File</label>
                                        <input type="file" name="document" class="super-input" style="padding: 6px !important;" required>
                                    </div>
                                    <button type="submit" name="upload_doc" class="btn btn-super w-100" style="background:#007bff;"><i class="fas fa-upload"></i> Upload</button>
                                </form>
                            </div>
                        </div>
                    </div>

                    <!-- Column 3: Full Roster matrix -->
                    <div class="col-lg-6">
                        <div class="card">
                            <div class="card-header"><i class="fas fa-globe text-primary"></i> Global Client Matrix</div>
                            <div class="card-body" style="height: 1050px; overflow-y: auto;">
                                
                                <?php if (count($db['clients']) == 0): ?>
                                    <div class="text-center text-muted py-5">
                                        <p>No clients registered globally.</p>
                                    </div>
                                <?php else: ?>

                                    <?php foreach($db['clients'] as $email => $clientData): 
                                        $client_files = [];
                                        $client_folders = [];
                                        $client_path = $doc_dir . $clientData['folder'];
                                        if (is_dir($client_path)) {
                                            $scanned = array_diff(scandir($client_path), array('..', '.'));
                                            foreach ($scanned as $item) {
                                                if (is_dir($client_path . '/' . $item)) {
                                                    $subscanned = array_diff(scandir($client_path . '/' . $item), array('..', '.'));
                                                    foreach($subscanned as $sub) {
                                                        if (!is_dir($client_path . '/' . $item . '/' . $sub)) {
                                                            $client_folders[$item][] = $sub;
                                                        }
                                                    }
                                                    if (!isset($client_folders[$item])) $client_folders[$item] = [];
                                                } else {
                                                    $client_files[] = $item;
                                                }
                                            }
                                        }
                                        $is_suspended = isset($clientData['suspended']) && $clientData['suspended'] === true;
                                        $suspended_files = isset($clientData['suspended_files']) ? $clientData['suspended_files'] : [];
                                        $suspended_folders = isset($clientData['suspended_folders']) ? $clientData['suspended_folders'] : [];
                                    ?>
                                        <div class="client-block">
                                            <div class="client-header" style="cursor: pointer; <?php if($is_suspended) echo 'background:#ffeeba; border-color:#ffdf7e;'; ?>" onclick="document.getElementById('client_body_<?php echo md5($email); ?>').classList.toggle('d-none'); document.getElementById('client_icon_<?php echo md5($email); ?>').classList.toggle('fa-chevron-down'); document.getElementById('client_icon_<?php echo md5($email); ?>').classList.toggle('fa-chevron-right');">
                                                <div>
                                                    <h5 style="color:#252b65; font-size:16px;">
                                                        <i id="client_icon_<?php echo md5($email); ?>" class="fas fa-chevron-right text-muted mr-2" style="font-size: 14px;"></i>
                                                        <?php echo htmlspecialchars($clientData['name']); ?>
                                                        <?php if($is_suspended): ?><span class="suspended-badge"><i class="fas fa-ban"></i> Suspended</span><?php endif; ?>
                                                    </h5>
                                                    <div class="text-sm mt-1" style="color:#666;">
                                                        <i class="fas fa-envelope"></i> <?php echo htmlspecialchars($email); ?>
                                                    </div>
                                                </div>
                                                <div onclick="event.stopPropagation();">
                                                    <a href="?action=toggle_suspend_client&client=<?php echo urlencode($email); ?>" class="btn btn-sm btn-<?php echo $is_suspended ? 'success' : 'warning'; ?>" title="Toggle Suspension"><i class="fas fa-<?php echo $is_suspended ? 'check' : 'pause'; ?>"></i></a>
                                                    <a href="?action=delete_client&client=<?php echo urlencode($email); ?>" onclick="return confirm('Delete client forever?');" class="btn btn-sm btn-outline-danger"><i class="fas fa-trash"></i></a>
                                                </div>
                                            </div>
                                            <div id="client_body_<?php echo md5($email); ?>" class="client-content p-3 d-none">
                                                
                                                <!-- Transfer Control -->
                                                <div class="p-2 mb-3" style="background:#f1f4f8; border-radius:4px; border:1px solid #e1e5eb;">
                                                    <form method="POST" style="display:flex; align-items:center; gap:10px;">
                                                        <input type="hidden" name="client_email" value="<?php echo htmlspecialchars($email); ?>">
                                                        <span class="text-sm font-weight-bold" style="color:#555; white-space:nowrap;">Managed By:</span>
                                                        <select name="new_admin" class="super-input mb-0" style="padding: 5px; height:auto; width:auto; flex-grow:1;">
                                                            <option value="">-- No Admin (Superadmin) --</option>
                                                            <?php foreach($db['admins'] as $e => $a): ?>
                                                                <option value="<?php echo htmlspecialchars($e); ?>" <?php if(isset($clientData['assigned_admin']) && $clientData['assigned_admin']==$e) echo 'selected'; ?>>
                                                                    <?php echo htmlspecialchars($a['name']); ?>
                                                                </option>
                                                            <?php endforeach; ?>
                                                        </select>
                                                        <button type="submit" name="transfer_client" class="btn btn-sm btn-dark">Transfer</button>
                                                    </form>
                                                </div>

                                                <?php if (count($client_files) > 0 || count($client_folders) > 0): ?>
                                                    <ul class="doc-list">
                                                        <?php foreach ($client_files as $file): 
                                                            $file_is_suspended = in_array($file, $suspended_files);
                                                        ?>
                                                            <li class="<?php if($file_is_suspended) echo 'file-suspended'; ?>">
                                                                <span class="text-sm">
                                                                    <i class="fas fa-file text-danger"></i> &nbsp; 
                                                                    <strong style="color:#444;"><?php echo htmlspecialchars($file); ?></strong>
                                                                </span>
                                                                <div>
                                                                    <a href="?action=toggle_suspend_file&client=<?php echo urlencode($email); ?>&file=<?php echo urlencode($file); ?>" class="btn btn-sm text-<?php echo $file_is_suspended ? 'success' : 'warning'; ?> py-0"><i class="fas fa-<?php echo $file_is_suspended ? 'eye' : 'eye-slash'; ?>"></i></a>
                                                                    <a href="?action=delete_file&client=<?php echo urlencode($email); ?>&file=<?php echo urlencode($file); ?>" onclick="return confirm('Immediately delete this file permanently?');" class="btn btn-sm text-danger py-0"><i class="fas fa-times"></i></a>
                                                                </div>
                                                            </li>
                                                        <?php endforeach; ?>
                                                        
                                                        <?php foreach ($client_folders as $folder_name => $folder_files): 
                                                            $folder_is_suspended = in_array($folder_name, $suspended_folders);
                                                            $folder_id = md5($email . $folder_name);
                                                        ?>
                                                            <li style="background: #f8f9fa; font-weight: bold; border-left: 3px solid #ffcc00; cursor: pointer; <?php if($folder_is_suspended) echo 'opacity: 0.6;'; ?>" onclick="document.getElementById('folder_body_<?php echo $folder_id; ?>').classList.toggle('d-none'); document.getElementById('folder_icon_<?php echo $folder_id; ?>').classList.toggle('fa-chevron-down'); document.getElementById('folder_icon_<?php echo $folder_id; ?>').classList.toggle('fa-chevron-right');">
                                                                <span class="text-sm">
                                                                    <i id="folder_icon_<?php echo $folder_id; ?>" class="fas fa-chevron-right text-muted mr-2"></i>
                                                                    <i class="fas fa-folder-open text-warning"></i> &nbsp; 
                                                                    <strong style="color:#444;"><?php echo htmlspecialchars($folder_name); ?></strong>
                                                                    <?php if($folder_is_suspended): ?> <span class="badge badge-warning bg-warning text-dark ml-2">Hidden</span> <?php endif; ?>
                                                                </span>
                                                                <div onclick="event.stopPropagation();">
                                                                    <a href="?action=toggle_suspend_folder&client=<?php echo urlencode($email); ?>&folder=<?php echo urlencode($folder_name); ?>" class="btn btn-sm text-<?php echo $folder_is_suspended ? 'success' : 'warning'; ?> py-0" style="box-shadow:none;"><i class="fas fa-<?php echo $folder_is_suspended ? 'eye' : 'eye-slash'; ?>"></i></a>
                                                                    <a href="?action=delete_folder&client=<?php echo urlencode($email); ?>&folder=<?php echo urlencode($folder_name); ?>" onclick="return confirm('Immediately delete this entire folder and all its contents?');" class="btn btn-sm text-danger py-0" title="Delete Folder"><i class="fas fa-trash"></i></a>
                                                                </div>
                                                            </li>
                                                            <div id="folder_body_<?php echo $folder_id; ?>" class="d-none w-100 p-0 m-0">
                                                                <?php foreach ($folder_files as $file): 
                                                                    $file_path_relative = $folder_name . '/' . $file;
                                                                    $file_is_suspended = in_array($file_path_relative, $suspended_files);
                                                                ?>
                                                                    <li class="<?php if($file_is_suspended) echo 'file-suspended'; ?>" style="padding-left: 35px; border-left: 1px dashed #ccc; margin-left: 10px;">
                                                                        <span class="text-sm">
                                                                            <i class="fas fa-file text-danger"></i> &nbsp; 
                                                                            <strong style="color:#444;"><?php echo htmlspecialchars($file); ?></strong>
                                                                        </span>
                                                                        <div style="margin-left: auto;">
                                                                            <a href="?action=toggle_suspend_file&client=<?php echo urlencode($email); ?>&file=<?php echo urlencode($file_path_relative); ?>" class="btn btn-sm text-<?php echo $file_is_suspended ? 'success' : 'warning'; ?> py-0"><i class="fas fa-<?php echo $file_is_suspended ? 'eye' : 'eye-slash'; ?>"></i></a>
                                                                            <a href="?action=delete_file&client=<?php echo urlencode($email); ?>&file=<?php echo urlencode($file_path_relative); ?>" onclick="return confirm('Immediately delete this file permanently?');" class="btn btn-sm text-danger py-0"><i class="fas fa-times"></i></a>
                                                                        </div>
                                                                    </li>
                                                                <?php endforeach; ?>
                                                            </div>
                                                        <?php endforeach; ?>
                                                    </ul>
                                                <?php else: ?>
                                                    <span class="text-muted text-sm d-block text-center p-2"><i class="fas fa-info-circle"></i> No documents uploaded.</span>
                                                <?php endif; ?>
                                            </div>
                                        </div>
                                    <?php endforeach; ?>

                                <?php endif; ?>
                            </div>
                        </div>

                    </div>
                </div>

                    </div>
                </div>

            <?php endif; ?>
        </div>
    </body>
</html>
