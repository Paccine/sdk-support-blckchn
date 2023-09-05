import { useState, useEffect } from 'react';

export default function Carousel() {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [autoplay, setAutoplay] = useState(true);
  const images = [
    'https://file.mir4global.com/mir4-brand-global/img/desktop/section4_aside/slide_xdraco.png',
    'https://file.mir4global.com/mir4-brand-global/img/desktop/section4_aside/slide_draco.png',
    'https://file.mir4global.com/mir4-brand-global/img/desktop/section4_aside/slide_dsp.png',
    'https://file.mir4global.com/mir4-brand-global/img/desktop/section4_aside/slide_nft.png',
    'https://file.mir4global.com/mir4-brand-global/img/desktop/section3/slide_5.jpg',
    'https://file.mir4global.com/mir4-brand-global/img/desktop/section4_aside/slide_xdraco.png'
  ];

  useEffect(() => {
    const interval = setInterval(() => {
      if (autoplay) {
        setCurrentIndex((prevIndex) => (prevIndex + 1) % images.length);
      }
    }, 3000);

    return () => clearInterval(interval);
  }, [autoplay, images.length]);

  return (
    <div id="blockchain" className="section">
      <h1>Simple World X Blockchain</h1>
      <div className="carousel">
        <img className="carousel-image" src={images[currentIndex]} alt="Carrossel" />
      </div>
      <div className="carousel-indicators">
        {images.map((_, index) => (
          <span
            key={index}
            className={`indicator ${index === currentIndex ? 'active' : ''}`}
            onClick={() => setCurrentIndex(index)}
          />
        ))}
        <span className="toggle" onClick={() => setAutoplay(!autoplay)}>
          {autoplay ? '⏸️' : '▶️'}
        </span>
      </div>
    </div>
  );
}
