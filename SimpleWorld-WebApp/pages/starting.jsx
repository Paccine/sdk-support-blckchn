import Head from 'next/head';
import Link from 'next/link';
import { useState } from 'react';

export default function Starting() {
  const [showPanel, setShowPanel] = useState('login'); // login, register, forgotPassword

  return (
    <div className="main-container">
      <Head>
        <title>Simple World</title>
        <link rel="icon" href="/img/icon.ico" type="image/x-icon" />
      </Head>
      <nav id="navbar">
        <div className="nav-content">
          <img src="/img/icon.ico" alt="Logo" width="30" height="30" />
          <div className="nav-links">
            <Link href="/"><a>Home</a></Link>
            <Link href="/#game"><a>Game</a></Link>
            <Link href="/#blockchain"><a>Blockchain</a></Link>
            <Link href="/#support"><a>Support</a></Link>
            <Link href="/starting"><a>Start Here</a></Link>
          </div>
        </div>
      </nav>
      <div className="content">
        <div className="panel-container">
          {showPanel === 'login' && (
            <div className="login-panel">
              <h2>Login</h2>
              <form>
                <label htmlFor="username">Usuário:</label>
                <input type="text" id="username" name="username" required />
                <label htmlFor="password">Senha:</label>
                <input type="password" id="password" name="password" required />
                <button type="submit" className="margin-button">Entrar</button>
              </form>
              <button onClick={() => setShowPanel('register')}>Registrar</button>
              <a href="#" className="forgot-password-link" onClick={() => setShowPanel('forgotPassword')}>Esqueceu sua senha?</a>
            </div>
          )}
          {showPanel === 'register' && (
            <div className="register-panel">
              <h2>Registro</h2>
              <form>
                <label htmlFor="username">Usuário:</label>
                <input type="text" id="username" name="username" required />
                <label htmlFor="password">Senha:</label>
                <input type="password" id="password" name="password" required />
                <label htmlFor="email">E-mail:</label>
                <input type="email" id="email" name="email" required />
                <label htmlFor="confirmEmail">Confirmar E-mail:</label>
                <input type="email" id="confirmEmail" name="confirmEmail" required />
                <button type="submit" className="margin-button">Registrar</button>
              </form>
              <button onClick={() => setShowPanel('login')}>Voltar ao Login</button>
            </div>
          )}
          {showPanel === 'forgotPassword' && (
            <div className="forgot-password-panel">
              <h2>Recuperar Senha</h2>
              <form>
                <label htmlFor="emailOrUsername">E-mail ou Usuário:</label>
                <input type="text" id="emailOrUsername" name="emailOrUsername" required />
                <button type="submit" className="margin-button">Recuperar</button>
              </form>
              <button onClick={() => setShowPanel('login')}>Voltar ao Login</button>
            </div>
          )}
        </div>
      </div>
      <footer>
        <div className="footer-content">
          <p className="copyright-text">Copyright © 2023 - Simple World - All Rights Reserved</p>
          <div className="social-icons">
            <a href="https://discord.gg/UKNGBwUEKC" target="_blank"><img className="discord-icon" src="/img/discord.png" /></a>
            <a href="https://www.instagram.com/simpleworld.online/" target="_blank"><img className="instagram-icon" src="/img/instagram.png" /></a>
            <a href="https://t.me/SimpleWorldCrypto" target="_blank"><img className="telegram-icon" src="/img/telegram.png" /></a>
          </div>
        </div>
      </footer>
    </div>
  );
}
