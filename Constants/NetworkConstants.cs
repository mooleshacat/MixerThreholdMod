using System;

namespace MixerThreholdMod_1_0_0.Constants
{
    /// <summary>
    /// Network and communication constants for HTTP, TCP, UDP, and protocol operations
    ///  IL2CPP COMPATIBLE: Compile-time constants safe for AOT compilation
    ///  .NET 4.8.1 COMPATIBLE: Uses explicit const declarations for maximum compatibility
    ///  THREAD SAFETY: All constants are immutable and thread-safe
    /// </summary>
    public static class NetworkConstants
    {
        #region HTTP Constants
        /// <summary>HTTP GET method</summary>
        public const string HTTP_METHOD_GET = "GET";

        /// <summary>HTTP POST method</summary>
        public const string HTTP_METHOD_POST = "POST";

        /// <summary>HTTP PUT method</summary>
        public const string HTTP_METHOD_PUT = "PUT";

        /// <summary>HTTP DELETE method</summary>
        public const string HTTP_METHOD_DELETE = "DELETE";

        /// <summary>HTTP PATCH method</summary>
        public const string HTTP_METHOD_PATCH = "PATCH";

        /// <summary>HTTP HEAD method</summary>
        public const string HTTP_METHOD_HEAD = "HEAD";

        /// <summary>HTTP OPTIONS method</summary>
        public const string HTTP_METHOD_OPTIONS = "OPTIONS";

        /// <summary>HTTP default timeout in milliseconds (30 seconds)</summary>
        public const int HTTP_DEFAULT_TIMEOUT_MS = 30000;

        /// <summary>HTTP retry attempts</summary>
        public const int HTTP_RETRY_ATTEMPTS = 3;

        /// <summary>HTTP retry delay in milliseconds</summary>
        public const int HTTP_RETRY_DELAY_MS = 1000;
        #endregion

        #region HTTP Status Codes
        /// <summary>HTTP 200 OK status code</summary>
        public const int HTTP_STATUS_OK = 200;

        /// <summary>HTTP 201 Created status code</summary>
        public const int HTTP_STATUS_CREATED = 201;

        /// <summary>HTTP 204 No Content status code</summary>
        public const int HTTP_STATUS_NO_CONTENT = 204;

        /// <summary>HTTP 400 Bad Request status code</summary>
        public const int HTTP_STATUS_BAD_REQUEST = 400;

        /// <summary>HTTP 401 Unauthorized status code</summary>
        public const int HTTP_STATUS_UNAUTHORIZED = 401;

        /// <summary>HTTP 403 Forbidden status code</summary>
        public const int HTTP_STATUS_FORBIDDEN = 403;

        /// <summary>HTTP 404 Not Found status code</summary>
        public const int HTTP_STATUS_NOT_FOUND = 404;

        /// <summary>HTTP 500 Internal Server Error status code</summary>
        public const int HTTP_STATUS_INTERNAL_SERVER_ERROR = 500;

        /// <summary>HTTP 502 Bad Gateway status code</summary>
        public const int HTTP_STATUS_BAD_GATEWAY = 502;

        /// <summary>HTTP 503 Service Unavailable status code</summary>
        public const int HTTP_STATUS_SERVICE_UNAVAILABLE = 503;

        /// <summary>HTTP 504 Gateway Timeout status code</summary>
        public const int HTTP_STATUS_GATEWAY_TIMEOUT = 504;
        #endregion

        #region HTTP Headers
        /// <summary>Content-Type HTTP header</summary>
        public const string HTTP_HEADER_CONTENT_TYPE = "Content-Type";

        /// <summary>Accept HTTP header</summary>
        public const string HTTP_HEADER_ACCEPT = "Accept";

        /// <summary>Authorization HTTP header</summary>
        public const string HTTP_HEADER_AUTHORIZATION = "Authorization";

        /// <summary>User-Agent HTTP header</summary>
        public const string HTTP_HEADER_USER_AGENT = "User-Agent";

        /// <summary>Content-Length HTTP header</summary>
        public const string HTTP_HEADER_CONTENT_LENGTH = "Content-Length";

        /// <summary>Accept-Encoding HTTP header</summary>
        public const string HTTP_HEADER_ACCEPT_ENCODING = "Accept-Encoding";

        /// <summary>Cache-Control HTTP header</summary>
        public const string HTTP_HEADER_CACHE_CONTROL = "Cache-Control";

        /// <summary>Connection HTTP header</summary>
        public const string HTTP_HEADER_CONNECTION = "Connection";

        /// <summary>Cookie HTTP header</summary>
        public const string HTTP_HEADER_COOKIE = "Cookie";

        /// <summary>Set-Cookie HTTP header</summary>
        public const string HTTP_HEADER_SET_COOKIE = "Set-Cookie";
        #endregion

        #region Content Types
        /// <summary>JSON content type</summary>
        public const string CONTENT_TYPE_JSON = "application/json";

        /// <summary>XML content type</summary>
        public const string CONTENT_TYPE_XML = "application/xml";

        /// <summary>Plain text content type</summary>
        public const string CONTENT_TYPE_TEXT = "text/plain";

        /// <summary>HTML content type</summary>
        public const string CONTENT_TYPE_HTML = "text/html";

        /// <summary>Form URL encoded content type</summary>
        public const string CONTENT_TYPE_FORM_URLENCODED = "application/x-www-form-urlencoded";

        /// <summary>Multipart form data content type</summary>
        public const string CONTENT_TYPE_MULTIPART_FORM = "multipart/form-data";

        /// <summary>Binary content type</summary>
        public const string CONTENT_TYPE_BINARY = "application/octet-stream";

        /// <summary>PDF content type</summary>
        public const string CONTENT_TYPE_PDF = "application/pdf";

        /// <summary>Image PNG content type</summary>
        public const string CONTENT_TYPE_IMAGE_PNG = "image/png";

        /// <summary>Image JPEG content type</summary>
        public const string CONTENT_TYPE_IMAGE_JPEG = "image/jpeg";
        #endregion

        #region TCP/UDP Constants
        /// <summary>Default TCP port</summary>
        public const int TCP_DEFAULT_PORT = 8080;

        /// <summary>Default UDP port</summary>
        public const int UDP_DEFAULT_PORT = 8081;

        /// <summary>TCP connection timeout in milliseconds</summary>
        public const int TCP_CONNECTION_TIMEOUT_MS = 5000;

        /// <summary>TCP read timeout in milliseconds</summary>
        public const int TCP_READ_TIMEOUT_MS = 10000;

        /// <summary>TCP write timeout in milliseconds</summary>
        public const int TCP_WRITE_TIMEOUT_MS = 10000;

        /// <summary>UDP socket timeout in milliseconds</summary>
        public const int UDP_SOCKET_TIMEOUT_MS = 5000;

        /// <summary>Maximum TCP connections</summary>
        public const int MAX_TCP_CONNECTIONS = 100;

        /// <summary>TCP buffer size</summary>
        public const int TCP_BUFFER_SIZE = 8192;

        /// <summary>UDP buffer size</summary>
        public const int UDP_BUFFER_SIZE = 4096;

        /// <summary>Socket keep-alive enabled</summary>
        public const bool SOCKET_KEEP_ALIVE_ENABLED = true;

        /// <summary>Socket no-delay enabled (Nagle algorithm disabled)</summary>
        public const bool SOCKET_NO_DELAY_ENABLED = true;
        #endregion

        #region Protocol Constants
        /// <summary>HTTP protocol identifier</summary>
        public const string PROTOCOL_HTTP = "HTTP";

        /// <summary>HTTPS protocol identifier</summary>
        public const string PROTOCOL_HTTPS = "HTTPS";

        /// <summary>TCP protocol identifier</summary>
        public const string PROTOCOL_TCP = "TCP";

        /// <summary>UDP protocol identifier</summary>
        public const string PROTOCOL_UDP = "UDP";

        /// <summary>WebSocket protocol identifier</summary>
        public const string PROTOCOL_WEBSOCKET = "WebSocket";

        /// <summary>FTP protocol identifier</summary>
        public const string PROTOCOL_FTP = "FTP";

        /// <summary>SMTP protocol identifier</summary>
        public const string PROTOCOL_SMTP = "SMTP";

        /// <summary>IMAP protocol identifier</summary>
        public const string PROTOCOL_IMAP = "IMAP";

        /// <summary>POP3 protocol identifier</summary>
        public const string PROTOCOL_POP3 = "POP3";

        /// <summary>SSH protocol identifier</summary>
        public const string PROTOCOL_SSH = "SSH";
        #endregion

        #region Encoding Constants
        /// <summary>UTF-8 encoding name</summary>
        public const string ENCODING_UTF8 = "UTF-8";

        /// <summary>UTF-16 encoding name</summary>
        public const string ENCODING_UTF16 = "UTF-16";

        /// <summary>ASCII encoding name</summary>
        public const string ENCODING_ASCII = "ASCII";

        /// <summary>Base64 encoding name</summary>
        public const string ENCODING_BASE64 = "Base64";

        /// <summary>URL encoding name</summary>
        public const string ENCODING_URL = "URL";

        /// <summary>HTML encoding name</summary>
        public const string ENCODING_HTML = "HTML";

        /// <summary>JSON encoding name</summary>
        public const string ENCODING_JSON = "JSON";

        /// <summary>XML encoding name</summary>
        public const string ENCODING_XML = "XML";
        #endregion

        #region Security Constants
        /// <summary>TLS 1.2 version</summary>
        public const string TLS_VERSION_12 = "TLS 1.2";

        /// <summary>TLS 1.3 version</summary>
        public const string TLS_VERSION_13 = "TLS 1.3";

        /// <summary>SSL certificate validation enabled</summary>
        public const bool SSL_CERT_VALIDATION_ENABLED = true;

        /// <summary>API key header name</summary>
        public const string API_KEY_HEADER = "X-API-Key";

        /// <summary>Bearer token prefix</summary>
        public const string BEARER_TOKEN_PREFIX = "Bearer ";

        /// <summary>Basic auth prefix</summary>
        public const string BASIC_AUTH_PREFIX = "Basic ";

        /// <summary>JWT token type</summary>
        public const string JWT_TOKEN_TYPE = "JWT";

        /// <summary>OAuth token type</summary>
        public const string OAUTH_TOKEN_TYPE = "OAuth";

        /// <summary>Session cookie name</summary>
        public const string SESSION_COOKIE_NAME = "SessionId";

        /// <summary>CSRF token header name</summary>
        public const string CSRF_TOKEN_HEADER = "X-CSRF-Token";
        #endregion

        #region Error Messages
        /// <summary>Network connection failed message</summary>
        public const string NETWORK_CONNECTION_FAILED = "Network connection failed";

        /// <summary>Request timeout message</summary>
        public const string NETWORK_REQUEST_TIMEOUT = "Network request timed out";

        /// <summary>Invalid response message</summary>
        public const string NETWORK_INVALID_RESPONSE = "Invalid network response";

        /// <summary>Authentication failed message</summary>
        public const string NETWORK_AUTH_FAILED = "Authentication failed";

        /// <summary>Server error message</summary>
        public const string NETWORK_SERVER_ERROR = "Server error occurred";

        /// <summary>Client error message</summary>
        public const string NETWORK_CLIENT_ERROR = "Client error occurred";

        /// <summary>SSL/TLS error message</summary>
        public const string NETWORK_SSL_ERROR = "SSL/TLS error occurred";

        /// <summary>DNS resolution failed message</summary>
        public const string NETWORK_DNS_FAILED = "DNS resolution failed";

        /// <summary>Protocol error message</summary>
        public const string NETWORK_PROTOCOL_ERROR = "Protocol error occurred";

        /// <summary>Rate limit exceeded message</summary>
        public const string NETWORK_RATE_LIMITED = "Rate limit exceeded";
        #endregion
    }
}