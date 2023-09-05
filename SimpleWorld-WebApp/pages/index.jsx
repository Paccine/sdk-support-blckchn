import Head from 'next/head';
import Carousel from '../components/Carousel';
import Link from 'next/link';

export default function Home() {
  return (
    <div className="main-container-home">
      <Head>
        <title>Simple World</title>
        <link rel="icon" href="/img/icon.ico" type="image/x-icon" />
      </Head>
      <nav id="navbar">
        <div className="nav-content">
          <img src="/img/icon.ico" alt="Logo" width="30" height="30" />
          <div className="nav-links">
            <a href="#home">Home</a>
            <a href="#game">Game</a>
            <a href="#blockchain">Blockchain</a>
            <a href="#support">Suporte</a>
            <Link href="/starting"><a>Start Here</a></Link> {/* Novo link para a página Starting */}

          </div>
        </div>
      </nav>
      <div className="content">
        <div id="home" className="section">
          <div className="main-logo-container">
            <img className="main-logo" src="/img/main-logo1.png" alt="Logo Principal" />
          </div>
          <div className="store-icons">
            <img className="app-store" src="/img/app-store.svg" />
            <img className="play-store" src="/img/play-store.png" />
          </div>
        </div>
        <div id="game" className="section"></div>
        <Carousel />
        <div id="support" className="section"></div>
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
