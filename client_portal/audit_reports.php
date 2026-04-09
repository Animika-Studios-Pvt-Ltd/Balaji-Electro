<?php
session_start();

// Ensure Superadmin is logged in
if (!isset($_SESSION['superadmin_logged_in'])) {
    header('Location: superadmin.php');
    exit;
}

$audit_file = 'audit_log.json';
$logs = [];
if (file_exists($audit_file)) {
    $logs = json_decode(file_get_contents($audit_file), true);
    if (!is_array($logs)) $logs = [];
}

// Handle Export to Excel (CSV format for universal compatibility)
if (isset($_POST['export_csv'])) {
    $start_date = strtotime($_POST['start_date']);
    // Add 86399 seconds to end date to hit 11:59:59 PM of that day
    $end_date = strtotime($_POST['end_date']) + 86399; 

    header('Content-Type: text/csv; charset=utf-8');
    header('Content-Disposition: attachment; filename="Balaji_Server_Audit_Report_'.date('Y-m-d').'.csv"');
    
    // Create a file pointer connected to the output stream
    $output = fopen('php://output', 'w');
    
    // Output the column headings
    fputcsv($output, ['Timestamp', 'Initiator (Admin/Client)', 'Action Type', 'Target Resource', 'Details / Downloaded File']);
    
    // Loop over the logs and filter by date
    foreach ($logs as $log) {
        $log_time = strtotime($log['timestamp']);
        if ($log_time >= $start_date && $log_time <= $end_date) {
            fputcsv($output, [
                $log['timestamp'],
                $log['actor'],
                $log['action_type'],
                $log['target'],
                $log['details']
            ]);
        }
    }
    fclose($output);
    exit;
}
?>
<!doctype html>
<html>
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>Audit Reports - Balaji Electro Controls</title>
        <link rel="stylesheet" type="text/css" href="../content/public/css/bootstrap.min.css">
        <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" media="screen">
        <style>
            body { background: #eef2f5; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }
            .topnav { background: #1a1e4a; padding: 15px 30px; display: flex; justify-content: space-between; align-items: center; color: white;}
            .btn-back { color: white; border: 1px solid rgba(255,255,255,0.3); padding: 6px 15px; border-radius: 4px; text-decoration: none;}
            .btn-back:hover { background: rgba(255,255,255,0.1); color: white; text-decoration: none;}
            
            .card { border: none; box-shadow: 0 4px 15px rgba(0,0,0,0.05); border-radius: 8px; margin-bottom: 30px;}
            .card-header { background: #fff; border-bottom: 2px solid #f1f1f1; padding: 20px 25px; font-weight: bold; color: #1a1e4a; font-size: 1.1rem; border-radius: 8px 8px 0 0;}
            
            .form-control { padding: 10px 15px; border: 1px solid #ced4da; border-radius: 4px; }
            .btn-excel { background: #217346; color: white; font-weight: bold; padding: 10px 20px; transition: 0.3s;}
            .btn-excel:hover { background: #175232; color: white; transform: translateY(-2px); box-shadow: 0 5px 15px rgba(33, 115, 70, 0.4);}
            
            /* Log Table styling */
            .table-container { max-height: 700px; overflow-y: auto; background: #0b0e27;}
            .table-dark th { position: sticky; top: 0; background: #1a1e4a; z-index: 10; border: none; padding: 15px;}
            .table-dark td { border-color: #1a1e4a; padding: 12px 15px; vertical-align: middle;}
        </style>
    </head>
    <body>
        
        <div class="topnav">
            <div>
                <h4 style="margin:0;"><i class="fas fa-shield-alt text-danger"></i> Global Audit</h4>
            </div>
            <div>
                <a href="superadmin.php" class="btn-back"><i class="fas fa-arrow-left"></i> Return to Core</a>
            </div>
        </div>

        <div class="container mt-5">
            
            <!-- Excel Export Controls -->
            <div class="row mb-4">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header"><i class="fas fa-file-excel" style="color:#217346;"></i> Download Excel Report</div>
                        <div class="card-body">
                            <form method="POST" class="row align-items-end">
                                <div class="col-md-4">
                                    <label class="font-weight-bold text-muted">Start Date</label>
                                    <input type="date" name="start_date" class="form-control" required>
                                </div>
                                <div class="col-md-4">
                                    <label class="font-weight-bold text-muted">End Date</label>
                                    <input type="date" name="end_date" class="form-control" required>
                                </div>
                                <div class="col-md-4">
                                    <button type="submit" name="export_csv" class="btn btn-excel w-100 mt-3 mt-md-0"><i class="fas fa-download"></i> Generate Excel File</button>
                                </div>
                            </form>
                            <p class="text-muted text-sm mt-3 mb-0"><i class="fas fa-info-circle"></i> This generates a universal Comma-Separated Values file format engineered to instantly open inside Microsoft Excel.</p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Full Log Viewer -->
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header"> Complete System History</div>
                        <div class="table-container p-0">
                            <table class="table table-dark table-hover mb-0" style="font-size: 14px;">
                                <thead>
                                    <tr>
                                        <th>Timestamp</th>
                                        <th>Initiator</th>
                                        <th>Action Type</th>
                                        <th>Target</th>
                                        <th>Detailed Records</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <?php if(count($logs) > 0): ?>
                                        <?php foreach($logs as $log): ?>
                                            <tr>
                                                <td style='color:#bbb; white-space:nowrap;'><?php echo htmlspecialchars($log['timestamp']); ?></td>
                                                <td style='color:#ff3366; font-weight:bold;'><?php echo htmlspecialchars($log['actor']); ?></td>
                                                <td><span class='badge badge-secondary bg-secondary'><?php echo htmlspecialchars($log['action_type']); ?></span></td>
                                                <td style='color:#5ac8fa;'><?php echo htmlspecialchars($log['target']); ?></td>
                                                <td><?php echo htmlspecialchars($log['details']); ?></td>
                                            </tr>
                                        <?php endforeach; ?>
                                    <?php else: ?>
                                        <tr><td colspan='5' class='text-center py-5'><i class="fas fa-folder-open fa-2x mb-2 text-muted"></i><br>No logs recorded in the system yet.</td></tr>
                                    <?php endif; ?>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </body>
</html>
