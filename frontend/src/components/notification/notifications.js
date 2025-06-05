'use client'
import { useState } from "react";

export function Notifications({ notifications, markAsRead, markAllAsRead, closePanel}) {

    return (
        <div className="w-96 h-full bg-blue-100 shadow-lg p-6">
            {/* Bouton de fermeture du panel */}
            <button
                onClick={closePanel}
                className="text-2xl font-semibold text-gray-500 hover:text-gray-900 "
            >
                &times; {/* Symbole de fermeture */}
            </button>

            <h2 className="text-xl font-semibold mt-4 text-center">Notifications</h2>


            {/* Bouton Mark All as Read */}
            <div className="mb-4">
                <button
                    onClick={markAllAsRead}

                >
                    mark all as read
                </button>
            </div>

            <div className="space-y-4">
                {notifications.map((notification) => (
                    <div
                        key={notification.id}
                        className={`p-4 rounded-lg border-none ${notification.read ? "bg-gray-200" : "bg-white"} shadow-sm`}
                        onClick={() => markAsRead(notification.id)}
                    >
                        <div className="flex justify-between items-center mb-2">
                            <div className="text-sm text-gray-500">
                                {new Date(notification.date).toLocaleString()}
                            </div>
                        </div>
                        <div className={`text-md ${notification.read ? "text-gray-400" : "text-black"}`}>
                            {notification.message}
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}
