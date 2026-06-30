import requests
import threading
import time
import json
import sys

# Configure stdout to use utf-8 to prevent charmap errors on Windows console
if sys.platform == 'win32':
    import codecs
    sys.stdout = codecs.getwriter('utf-8')(sys.stdout.buffer, 'replace')

url = "http://localhost:8080/mcp"
print("1. Getting session ID...")
try:
    res = requests.get(url, timeout=5)
    session_id = res.headers.get("mcp-session-id")
    print(f"Retrieved session ID: {session_id}")
    
    if not session_id:
        print("Could not retrieve session ID.")
        sys.exit(1)
        
    headers = {
        'Accept': 'application/json, text/event-stream',
        'mcp-session-id': session_id
    }
    
    # Background thread to maintain SSE connection
    def keep_sse_alive():
        try:
            sse_res = requests.get(url, headers=headers, stream=True)
            for line in sse_res.iter_lines():
                if line:
                    decoded = line.decode('utf-8', errors='ignore')
                    print(f"[SSE Event] {decoded}")
        except Exception as e:
            print(f"SSE Thread exited: {e}")

    t = threading.Thread(target=keep_sse_alive)
    t.daemon = True
    t.start()
    
    # Wait for SSE connection to establish
    time.sleep(2)
    
    # 3. Call tool via POST
    post_headers = {
        'Content-Type': 'application/json',
        'Accept': 'application/json, text/event-stream',
        'mcp-session-id': session_id
    }
    
    # First we must initialize the MCP session!
    init_payload = {
        "jsonrpc": "2.0",
        "id": 1,
        "method": "initialize",
        "params": {
            "protocolVersion": "2024-11-05",
            "capabilities": {},
            "clientInfo": {
                "name": "PythonTestClient",
                "version": "1.0.0"
            }
        }
    }
    
    print("\n2. Initializing MCP session...")
    init_res = requests.post(url, json=init_payload, headers=post_headers, timeout=5)
    print(f"Initialize Response Status: {init_res.status_code}")
    print(init_res.text.encode('ascii', errors='ignore').decode('ascii')[:300] + "...")
    
    # Send initialized notification
    initialized_payload = {
        "jsonrpc": "2.0",
        "method": "notifications/initialized"
    }
    print("\n3. Sending initialized notification...")
    requests.post(url, json=initialized_payload, headers=post_headers, timeout=5)
    
    # Now call tools/call for read_console!
    call_payload = {
        "jsonrpc": "2.0",
        "id": 2,
        "method": "tools/call",
        "params": {
            "name": "read_console",
            "arguments": {
                "action": "get",
                "count": 30,
                "include_stacktrace": True,
                "format": "detailed"
            }
        }
    }
    
    print("\n4. Calling read_console tool...")
    call_res = requests.post(url, json=call_payload, headers=post_headers, timeout=15)
    print(f"Call Response Status: {call_res.status_code}")
    with open("console_logs.txt", "w", encoding="utf-8") as f:
        f.write(call_res.text)
    print("Saved response to console_logs.txt")

except Exception as e:
    print(f"Error: {e}")
