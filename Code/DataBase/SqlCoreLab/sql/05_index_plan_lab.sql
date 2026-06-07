-- Index and execution plan lab.
-- Run each block separately and compare EXPLAIN ANALYZE output.

-- Baseline: inspect row counts.
SELECT count(*) AS batch_count FROM lab_test_batches;
SELECT count(*) AS alarm_count FROM lab_alarm_events;

-- 1. Query by device and time before adding the matching index.
EXPLAIN ANALYZE
SELECT
    id,
    batch_code,
    device_id,
    status,
    started_at
FROM lab_test_batches
WHERE device_id = 42
ORDER BY started_at DESC
LIMIT 20;

-- 2. Add a composite index for the access path.
CREATE INDEX IF NOT EXISTS idx_lab_test_batches_device_started
ON lab_test_batches (device_id, started_at DESC);

ANALYZE lab_test_batches;

EXPLAIN ANALYZE
SELECT
    id,
    batch_code,
    device_id,
    status,
    started_at
FROM lab_test_batches
WHERE device_id = 42
ORDER BY started_at DESC
LIMIT 20;

-- 3. Column order experiment.
CREATE INDEX IF NOT EXISTS idx_lab_test_batches_started_device
ON lab_test_batches (started_at DESC, device_id);

ANALYZE lab_test_batches;

EXPLAIN ANALYZE
SELECT
    id,
    batch_code,
    device_id,
    status,
    started_at
FROM lab_test_batches
WHERE device_id = 42
ORDER BY started_at DESC
LIMIT 20;

-- 4. Low-selectivity column experiment.
CREATE INDEX IF NOT EXISTS idx_lab_test_batches_status
ON lab_test_batches (status);

ANALYZE lab_test_batches;

EXPLAIN ANALYZE
SELECT
    id,
    batch_code,
    status
FROM lab_test_batches
WHERE status = 'success'
LIMIT 1000;

-- 5. Function wrapped column.
EXPLAIN ANALYZE
SELECT
    id,
    batch_code,
    started_at
FROM lab_test_batches
WHERE CAST(started_at AS date) = DATE '2025-01-15'
ORDER BY started_at;

EXPLAIN ANALYZE
SELECT
    id,
    batch_code,
    started_at
FROM lab_test_batches
WHERE started_at >= TIMESTAMP '2025-01-15 00:00:00'
  AND started_at < TIMESTAMP '2025-01-16 00:00:00'
ORDER BY started_at;

-- 6. Deep offset pagination.
EXPLAIN ANALYZE
SELECT
    id,
    batch_code,
    started_at
FROM lab_test_batches
ORDER BY started_at DESC, id DESC
OFFSET 50000
LIMIT 20;

-- 7. Keyset pagination.
-- Replace the boundary values with the last row from the previous page in real usage.
EXPLAIN ANALYZE
SELECT
    id,
    batch_code,
    started_at
FROM lab_test_batches
WHERE (started_at, id) < (TIMESTAMP '2025-02-05 00:00:00', 50000)
ORDER BY started_at DESC, id DESC
LIMIT 20;

CREATE INDEX IF NOT EXISTS idx_lab_test_batches_started_id
ON lab_test_batches (started_at DESC, id DESC);

ANALYZE lab_test_batches;

EXPLAIN ANALYZE
SELECT
    id,
    batch_code,
    started_at
FROM lab_test_batches
WHERE (started_at, id) < (TIMESTAMP '2025-02-05 00:00:00', 50000)
ORDER BY started_at DESC, id DESC
LIMIT 20;
