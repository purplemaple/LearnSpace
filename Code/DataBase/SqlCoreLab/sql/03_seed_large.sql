-- Larger data set for index and execution plan experiments.
-- Run after 01_schema.sql. This script appends data.

INSERT INTO lab_devices (device_code, device_name, model, line_code, status)
SELECT
    'AUTO-DEV-' || to_char(gs, 'FM0000'),
    'Auto Device ' || gs,
    CASE WHEN gs % 3 = 0 THEN 'SL-A' WHEN gs % 3 = 1 THEN 'RM-B' ELSE 'RD-D' END,
    'LINE-' || chr(65 + (gs % 5)),
    CASE WHEN gs % 17 = 0 THEN 'maintenance' WHEN gs % 29 = 0 THEN 'disabled' ELSE 'idle' END
FROM generate_series(1, 200) AS gs
ON CONFLICT (device_code) DO NOTHING;

INSERT INTO lab_operators (operator_code, operator_name, team_name)
SELECT
    'AUTO-OP-' || to_char(gs, 'FM000'),
    'Auto Operator ' || gs,
    'Team-' || chr(65 + (gs % 6))
FROM generate_series(1, 50) AS gs
ON CONFLICT (operator_code) DO NOTHING;

INSERT INTO lab_test_batches (
    batch_code,
    device_id,
    operator_id,
    status,
    started_at,
    finished_at,
    sample_count,
    error_code,
    remark
)
SELECT
    'LOAD-' || to_char(gs, 'FM000000'),
    ((gs - 1) % (SELECT count(*) FROM lab_devices)) + 1,
    ((gs - 1) % (SELECT count(*) FROM lab_operators)) + 1,
    CASE
        WHEN gs % 97 = 0 THEN 'cancelled'
        WHEN gs % 11 = 0 THEN 'failed'
        WHEN gs % 997 = 0 THEN 'running'
        ELSE 'success'
    END,
    TIMESTAMP '2025-01-01 00:00:00' + (gs || ' minutes')::INTERVAL,
    CASE
        WHEN gs % 997 = 0 THEN NULL
        ELSE TIMESTAMP '2025-01-01 00:00:00' + (gs || ' minutes')::INTERVAL + (((gs % 120) + 5) || ' seconds')::INTERVAL
    END,
    (gs % 120) + 1,
    CASE
        WHEN gs % 11 = 0 THEN 'E-' || to_char(gs % 31, 'FM000')
        WHEN gs % 97 = 0 THEN 'C-' || to_char(gs % 13, 'FM000')
        ELSE NULL
    END,
    NULL
FROM generate_series(1, 100000) AS gs
ON CONFLICT (batch_code) DO NOTHING;

INSERT INTO lab_alarm_events (
    device_id,
    batch_id,
    alarm_code,
    alarm_level,
    alarm_message,
    occurred_at,
    cleared_at
)
SELECT
    b.device_id,
    b.id,
    COALESCE(b.error_code, 'W-' || to_char(b.id % 19, 'FM000')),
    CASE
        WHEN b.status = 'failed' THEN 'error'
        WHEN b.status = 'cancelled' THEN 'warning'
        ELSE 'info'
    END,
    CASE
        WHEN b.status = 'failed' THEN 'Large seed generated failure'
        WHEN b.status = 'cancelled' THEN 'Large seed generated cancellation'
        ELSE 'Large seed generated warning'
    END,
    b.started_at + INTERVAL '30 seconds',
    b.started_at + INTERVAL '90 seconds'
FROM lab_test_batches b
WHERE b.batch_code LIKE 'LOAD-%'
  AND (b.status <> 'success' OR b.id % 223 = 0)
  AND NOT EXISTS (
      SELECT 1
      FROM lab_alarm_events a
      WHERE a.batch_id = b.id
  );

ANALYZE lab_devices;
ANALYZE lab_operators;
ANALYZE lab_test_batches;
ANALYZE lab_alarm_events;
