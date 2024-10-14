import React, { useState } from 'react';
import {API_BASE_URL} from "../../config.ts";
import '../App.css';

interface CreatePostProps {
    onPostCreated: () => void;
}

const CreatePost: React.FC<CreatePostProps> = ({ onPostCreated }) => {
    const [username, setUsername] = useState<string>('');
    const [content, setContent] = useState<string>('');
    const [charCount, setCharCount] = useState<number>(0);
    const [error, setError] = useState<string>('');
    const maxCount : number = 140;
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!username || !content) {
            setError('Both fields are required');
            return;
        }

        const newPost = { username: username, content: content };

        try {
            const response = await fetch(`${API_BASE_URL}/posts`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(newPost),
            });

            if (!response.ok) {
                throw new Error('Error creating post');
            }

            setUsername('');
            setContent('');
            onPostCreated(); // rrefresh the post list
        } catch (err: any) {
            setError(err.message);
        }
    };

    return (
        <div>
            <h1>Submit New Post</h1>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Username: </label>
                    <br/>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                </div>
                <div>
                    <label>Contents {charCount}/140: </label>
                    <br/>
                    <textarea
                        value={content}
                        onChange={(e) => {
                            setContent(e.target.value);
                            setCharCount(e.target.value.length);
                            if (e.target.value.length > maxCount) {
                                setError("Content is too long")
                            }
                        }}
                    ></textarea>
                </div>
                <button type="submit" disabled={charCount > maxCount}>Create Post</button>
            </form>
        </div>
    );
};

export default CreatePost;