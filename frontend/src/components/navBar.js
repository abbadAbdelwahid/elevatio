


'use client'
import React, { useState, useEffect, useRef } from 'react';
import Image from 'next/image';

const ImageCarousel = () => {

    const [currentSlide, setCurrentSlide] = useState(0);
    const [autoPlay, setAutoPlay] = useState(true);
    const [touchStart, setTouchStart] = useState(0);
    const [touchEnd, setTouchEnd] = useState(0);
    const carouselRef = useRef(null);

    const images = [
        { src: '/images/acceuil/carousel1.png', alt: "Feature 1" },
        { src: '/images/acceuil/carousel2.png', alt: "Feature 2" },
        { src: '/images/acceuil/carousel3.png', alt: "Feature 3" },
    ];

    // Gestion du swipe pour mobile
    const handleTouchStart = (e) => {
        setTouchStart(e.targetTouches[0].clientX);
    };

    const handleTouchMove = (e) => {
        setTouchEnd(e.targetTouches[0].clientX);
    };

    const handleTouchEnd = () => {
        if (touchStart - touchEnd > 100) {
            // Swipe gauche
            goToNext();
        }

        if (touchStart - touchEnd < -100) {
            // Swipe droite
            goToPrev();
        }
    };

    // Défilement automatique
    useEffect(() => {
        let interval;
        if (autoPlay) {
            interval = setInterval(() => {
                setCurrentSlide((prev) => (prev + 1) % images.length);
            }, 4000);
        }
        return () => clearInterval(interval);
    }, [autoPlay, images.length]);

    const goToSlide = (index) => {
        setCurrentSlide(index);
        pauseAndResumeAutoPlay();
    };

    const goToPrev = () => {
        setCurrentSlide((prev) => (prev - 1 + images.length) % images.length);
        pauseAndResumeAutoPlay();
    };

    const goToNext = () => {
        setCurrentSlide((prev) => (prev + 1) % images.length);
        pauseAndResumeAutoPlay();
    };

    const pauseAndResumeAutoPlay = () => {
        setAutoPlay(false);
        setTimeout(() => setAutoPlay(true), 10000);
    };
    // className="max-w-4xl mx-auto rounded-lg overflow-hidden shadow-lg
    return (
        <div
            className="relative flex flex-col items-center group"
            ref={carouselRef}
            onTouchStart={handleTouchStart}
            onTouchMove={handleTouchMove}
            onTouchEnd={handleTouchEnd}
        >
            {/* Conteneur principal */}
            <div className="relative w-full max-w-3xl flex items-center ">
                {/* Flèche gauche intégrée à l'image */}
                <button
                    type="button"
                    className="absolute left-2 z-30 h-8 w-8 flex items-center justify-center bg-white/80 rounded-full shadow-md hover:bg-white transition-all -translate-y-1/2 top-1/2"
                    onClick={goToPrev}
                    aria-label="Previous slide"
                >
                    <svg className="w-4 h-4 text-gray-800" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 6 10">
                        <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 1 1 5l4 4" />
                    </svg>
                </button>

                {/* Conteneur de l'image */}
                <div className="relative w-full h-96 overflow-hidden rounded-4xl mx-4 border-2 border-gray-100 ">
                    {images.map((image, index) => (
                        <div
                            key={index}
                            className={`duration-500 ease-out absolute inset-0 transition-opacity ${
                                index === currentSlide ? 'opacity-100' : 'opacity-0'
                            }`}
                        >
                            <div className="absolute inset-0 flex items-center justify-center bg-gray-50">
                                <Image
                                    src={image.src}
                                    alt={image.alt}
                                    className="object-cover cursor-pointer"
                                    fill
                                    priority={index === 0}
                                    sizes="(max-width: 768px) 100vw, 800px"
                                />
                            </div>
                        </div>
                    ))}
                </div>

                {/* Flèche droite intégrée à l'image */}
                <button
                    type="button"
                    className="absolute right-2 z-30 h-8 w-8 flex items-center justify-center bg-white/80 rounded-full shadow-md hover:bg-white transition-all -translate-y-1/2 top-1/2"
                    onClick={goToNext}
                    aria-label="Next slide"
                >
                    <svg className="w-4 h-4 text-gray-800" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 6 10">
                        <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="m1 9 4-4-4-4" />
                    </svg>
                </button>
            </div>

            {/* Points de navigation */}
            <div className="mt-4 flex space-x-2">
                {images.map((_, index) => (
                    <button
                        key={index}
                        className={`h-2 rounded-full transition-all duration-300 ${
                            index === currentSlide ? 'w-6 bg-gray-800' : 'w-3 bg-gray-300 hover:bg-gray-400'
                        }`}
                        onClick={() => goToSlide(index)}
                    />
                ))}
            </div>
        </div>
    )
};

export default ImageCarousel;


