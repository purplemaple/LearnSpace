-- Query drills for SQL semantics and interview practice.

-- 1. Latest batch for each device.
WITH ranked_batches AS (
    SELECT
        b.*,
        ROW_NUMBER() OVER (
            PARTITION BY b.device_id
            ORDER BY b.started_at DESC, b.id DESC
        ) AS rn
    FROM lab_test_batches b
)
SELECT
    d.device_code,
    d.device_name,
    rb.batch_code,
    rb.status,
    rb.started_at
FROM ranked_batches rb
JOIN lab_devices d ON d.id = rb.device_id
WHERE rb.rn = 1
ORDER BY d.device_code;

-- 2. Success rate by device.
SELECT
    d.device_code,
    count(*) AS total_count,
    count(*) FILTER (WHERE b.status = 'success') AS success_count,
    round(count(*) FILTER (WHERE b.status = 'success')::numeric / count(*) * 100, 2) AS success_rate
FROM lab_devices d
JOIN lab_test_batches b ON b.device_id = d.id
GROUP BY d.id, d.device_code
ORDER BY success_rate ASC, total_count DESC;

-- 3. Devices that have no batches.
SELECT
    d.id,
    d.device_code,
    d.device_name
FROM lab_devices d
LEFT JOIN lab_test_batches b ON b.device_id = d.id
WHERE b.id IS NULL
ORDER BY d.id;

-- 4. Batch rows that have no step detail.
SELECT
    b.id,
    b.batch_code,
    b.status
FROM lab_test_batches b
LEFT JOIN lab_batch_steps s ON s.batch_id = b.id
WHERE s.id IS NULL
ORDER BY b.id
LIMIT 50;

-- 5. Alarm count top 10.
SELECT
    d.device_code,
    a.alarm_code,
    count(*) AS alarm_count
FROM lab_alarm_events a
JOIN lab_devices d ON d.id = a.device_id
GROUP BY d.device_code, a.alarm_code
ORDER BY alarm_count DESC, d.device_code
LIMIT 10;

-- 6. Time gap between adjacent batches by device.
SELECT
    device_id,
    batch_code,
    started_at,
    started_at - LAG(started_at) OVER (
        PARTITION BY device_id
        ORDER BY started_at
    ) AS gap_from_previous
FROM lab_test_batches
ORDER BY device_id, started_at
LIMIT 100;

-- 7. Duplicate business key check.
SELECT
    batch_code,
    count(*) AS duplicate_count
FROM lab_test_batches
GROUP BY batch_code
HAVING count(*) > 1;

-- 8. Conditional aggregation by day.
SELECT
    CAST(started_at AS date) AS run_date,
    count(*) AS total_count,
    count(*) FILTER (WHERE status = 'success') AS success_count,
    count(*) FILTER (WHERE status = 'failed') AS failed_count,
    count(*) FILTER (WHERE status = 'cancelled') AS cancelled_count
FROM lab_test_batches
GROUP BY CAST(started_at AS date)
ORDER BY run_date;

-- 9. Top 3 longest batches for each device.
WITH ranked AS (
    SELECT
        b.device_id,
        b.batch_code,
        b.started_at,
        b.finished_at,
        b.finished_at - b.started_at AS elapsed_time,
        ROW_NUMBER() OVER (
            PARTITION BY b.device_id
            ORDER BY b.finished_at - b.started_at DESC NULLS LAST
        ) AS rn
    FROM lab_test_batches b
)
SELECT
    d.device_code,
    r.batch_code,
    r.elapsed_time,
    r.rn
FROM ranked r
JOIN lab_devices d ON d.id = r.device_id
WHERE r.rn <= 3
ORDER BY d.device_code, r.rn;

-- 10. Anti-join using NOT EXISTS.
SELECT
    d.id,
    d.device_code
FROM lab_devices d
WHERE NOT EXISTS (
    SELECT 1
    FROM lab_alarm_events a
    WHERE a.device_id = d.id
)
ORDER BY d.id;
