// src/App.tsx
import React, { useState } from 'react';
import './App.css'; 
import PostList from './components/PostList';
import PostDetails from './components/PostDetails';
import CreatePost from './components/PostCreate';

const App: React.FC = () => {
    const [view, setView] = useState<'list' | 'details' | 'create'>('list');
    const [selectedPostId, setSelectedPostId] = useState<number | null>(null);

    const handlePostCreated = () => {
        setView('list'); 
    };

    const handleSelectPost = (postId: number) => {
        setSelectedPostId(postId);
        setView('details');
    };

    const handleBackToList = () => {
        setView('list');
        setSelectedPostId(null);
    };

    return (
        <div>
            {view === 'list' && (
                <>
                    <button onClick={() => setView('create')}>Create New Post</button>
                    <PostList selectPost={handleSelectPost} />
                </>
            )}
            {view === 'details' && selectedPostId !== null && (
                <PostDetails postId={selectedPostId} backToList={handleBackToList} />
            )}
            {view === 'create' && <CreatePost onPostCreated={handlePostCreated} />}
        </div>
    );
};

export default App;