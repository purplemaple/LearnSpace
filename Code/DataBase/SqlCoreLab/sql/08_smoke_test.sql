-- Smoke test after running schema and seed scripts.

SELECT 'lab_devices' AS table_name, count(*) AS row_count FROM lab_devices
UNION ALL
SELECT 'lab_operators', count(*) FROM lab_operators
UNION ALL
SELECT 'lab_test_batches', count(*) FROM lab_test_batches
UNION ALL
SELECT 'lab_batch_steps', count(*) FROM lab_batch_steps
UNION ALL
SELECT 'lab_alarm_events', count(*) FROM lab_alarm_events
ORDER BY table_name;

SELECT
    d.device_code,
    count(b.id) AS batch_count,
    count(a.id) AS alarm_count
FROM lab_devices d
LEFT JOIN lab_test_batches b ON b.device_id = d.id
LEFT JOIN lab_alarm_events a ON a.device_id = d.id
GROUP BY d.id, d.device_code
ORDER BY d.device_code
LIMIT 10;
