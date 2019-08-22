﻿/***************************************************************************** 
  ____    _    _____    _      ____             _        _   
 / ___|  / \  | ____|  / \    / ___|  ___   ___| | _____| |_ 
 \___ \ / _ \ |  _|   / _ \   \___ \ / _ \ / __| |/ / _ \ __|
  ___) / ___ \| |___ / ___ \   ___) | (_) | (__|   <  __/ |_ 
 |____/_/   \_\_____/_/   \_\ |____/ \___/ \___|_|\_\___|\__|
                                                             

*Copyright (c) 2018 yswenli All Rights Reserved.
*CLR版本： 2.1.4
*机器名称：WENLI-PC
*公司名称：wenli
*命名空间：SAEA.Sockets
*文件名： SocketOptionBuilder
*版本号： v5.0.0.1
*唯一标识：ef84e44b-6fa2-432e-90a2-003ebd059303
*当前的用户域：WENLI-PC
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2018/3/1 15:54:21
*描述：
*
*=====================================================================
*修改标记
*修改时间：2018/3/1 15:54:21
*修改人： yswenli
*版本号： v5.0.0.1
*描述：
*****************************************************************************/
using System;
using SAEA.Common;
using SAEA.Sockets.Interface;
using SAEA.Sockets.Model;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using SAEA.Sockets.Base;

namespace SAEA.Sockets
{
    /// <summary>
    /// socket选项创造器
    /// </summary>
    public class SocketOptionBuilder
    {

        ISocketOption _socketOption;

        /// <summary>
        /// socket选项创造器
        /// </summary>
        public SocketOptionBuilder()
        {
            _socketOption = new SocketOption();
        }

        /// <summary>
        /// 返回一个SocketOptionBuilder实例
        /// </summary>
        public static SocketOptionBuilder Instance
        {
            get
            {
                return new SocketOptionBuilder();
            }
        }

        /// <summary>
        /// 设置socket类型
        /// </summary>
        /// <param name="socketType"></param>
        /// <returns></returns>
        public SocketOptionBuilder SetSocket(SAEASocketType socketType = SAEASocketType.Tcp)
        {
            _socketOption.SocketType = socketType;
            return this;
        }

        /// <summary>
        /// 启用iocp
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public SocketOptionBuilder UseIocp(IContext context)
        {
            if (_socketOption.WithSsl) throw new Exception("ssl模式下暂不支持icop");
            _socketOption.Context = context;
            _socketOption.SocketType = SAEASocketType.Tcp;
            _socketOption.UseIocp = true;
            return this;
        }

        /// <summary>
        /// 启用iocp
        /// </summary>
        /// <returns></returns>
        public SocketOptionBuilder UseIocp()
        {
            if (_socketOption.WithSsl) throw new Exception("ssl模式下暂不支持icop");
            _socketOption.Context = new BaseContext();
            _socketOption.SocketType = SAEASocketType.Tcp;
            _socketOption.UseIocp = true;
            return this;
        }

        /// <summary>
        /// 使用流模式
        /// </summary>
        /// <returns></returns>
        public SocketOptionBuilder UseStream()
        {
            _socketOption.SocketType = SAEASocketType.Tcp;
            _socketOption.UseIocp = false;
            return this;
        }

        /// <summary>
        /// 是否延时
        /// </summary>
        /// <returns></returns>
        public SocketOptionBuilder SetDelay()
        {
            _socketOption.NoDelay = false;
            return this;
        }

        /// <summary>
        /// 使用ssl
        /// </summary>
        /// <param name="sslProtocols"></param>
        /// <param name="pfxFilePath"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public SocketOptionBuilder WithSsl(SslProtocols sslProtocols, string pfxFilePath = "*.pfx", string pwd = "")
        {
            if (_socketOption.UseIocp) throw new Exception("暂不支持此模式下的ssl");
            _socketOption.SslProtocol = sslProtocols;
            if (!string.IsNullOrEmpty(pfxFilePath))
            {
                if (!FileHelper.Exists(pfxFilePath))
                {
                    throw new Exception("cenFilePath设置有误，找不到该证书文件!");
                }
                _socketOption.X509Certificate2 = new X509Certificate2(pfxFilePath, pwd, X509KeyStorageFlags.PersistKeySet);
                _socketOption.WithSsl = true;
            }
            return this;
        }

        /// <summary>
        /// 使用ssl
        /// </summary>
        /// <param name="x509Certificate2"></param>
        /// <param name="sslProtocols"></param>
        /// <returns></returns>
        public SocketOptionBuilder WithSsl(X509Certificate2 x509Certificate2, SslProtocols sslProtocols)
        {
            if (_socketOption.UseIocp) throw new Exception("暂不支持此模式下的ssl");
            _socketOption.SslProtocol = sslProtocols;
            if (x509Certificate2 != null)
            {
                _socketOption.X509Certificate2 = x509Certificate2;
                _socketOption.WithSsl = true;
            }
            return this;
        }

        /// <summary>
        /// 使用ipv6
        /// </summary>
        /// <returns></returns>
        public SocketOptionBuilder UseIPv6()
        {
            _socketOption.UseIPV6 = true;
            return this;
        }

        /// <summary>
        /// 设置ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public SocketOptionBuilder SetIP(string ip)
        {
            _socketOption.IP = ip;
            return this;
        }

        /// <summary>
        /// 设置端口
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public SocketOptionBuilder SetPort(int port = 39654)
        {
            _socketOption.Port = port;
            return this;
        }

        /// <summary>
        /// 读取缓冲区大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public SocketOptionBuilder SetReadBufferSize(int size = 1024)
        {
            _socketOption.ReadBufferSize = size;
            return this;
        }

        /// <summary>
        /// 写入缓冲区大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public SocketOptionBuilder SetWriteBufferSize(int size = 1024)
        {
            _socketOption.WriteBufferSize = size;
            return this;
        }

        /// <summary>
        /// 服务器中支持的客户端数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public SocketOptionBuilder SetCount(int count = 100)
        {
            _socketOption.Count = count;
            return this;
        }

        /// <summary>
        /// 是否端口复用
        /// </summary>
        /// <param name="reusePort"></param>
        /// <returns></returns>
        public SocketOptionBuilder ReusePort(bool reusePort = true)
        {
            _socketOption.ReusePort = reusePort;
            return this;
        }

        /// <summary>
        /// 操作超时
        /// </summary>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public SocketOptionBuilder SetTimeOut(int timeOut = 60 * 1000)
        {
            _socketOption.TimeOut = timeOut;
            return this;
        }

        public ISocketOption Build()
        {
            return _socketOption;
        }
    }
}
