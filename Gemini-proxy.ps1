# 设置 Node.js fetch 代理
$env:NODE_OPTIONS='-r global-agent/bootstrap'
$env:GLOBAL_AGENT_HTTP_PROXY='http://127.0.0.1:8889'
$env:GLOBAL_AGENT_HTTPS_PROXY='http://127.0.0.1:8889'

# 执行 Gemini CLI，传递所有参数
gemini @args

# 保持窗口打开，便于查看输出（可选）
Read-Host "Press Enter to exit"
