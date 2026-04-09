<?php
function log_action($action_type, $actor, $target, $details = '') {
    $log_file = __DIR__ . '/audit_log.json';
    
    // Set time zone
    date_default_timezone_set('Asia/Kolkata');
    
    // Initialize if missing
    if (!file_exists($log_file)) {
        file_put_contents($log_file, json_encode([]));
    }
    
    $logs = json_decode(file_get_contents($log_file), true);
    if (!is_array($logs)) $logs = [];
    
    $entry = [
        'timestamp' => date("Y-m-d h:i A"),
        'action_type' => $action_type,
        'actor' => $actor,
        'target' => $target,
        'details' => $details
    ];
    
    array_unshift($logs, $entry); // Add to top
    
    // Prune history to max 500 records
    if (count($logs) > 500) {
        $logs = array_slice($logs, 0, 500);
    }
    
    file_put_contents($log_file, json_encode($logs, JSON_PRETTY_PRINT));
}
?>
