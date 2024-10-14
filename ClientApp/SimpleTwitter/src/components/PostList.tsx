import React, { useEffect, useState } from 'react';
import { Post } from '../models/post.ts';
import {API_BASE_URL} from "../../config.ts";
import '../App.css';

interface PostListProps {
    selectPost: (id: number) => void;
}

const PostList: React.FC<PostListProps> = ({ selectPost }) => {
    const [posts, setPosts] = useState<Post[]>([]);

    useEffect(() => {
        fetch(`${API_BASE_URL}/posts`)
            .then(response => response.json())
            .then(data => setPosts(data))
            .catch(error => console.error('Error fetching posts:', error));
    }, []);

    return (
        <div>
            <h1>Recent posts:</h1>
            <ul>
                {posts.map(post => {
                    const date = new Date(post.createdDate);
                    const formattedDate = date.toLocaleString();
                    
                    return (
                        <li key={post.id}>
                            <h3>{post.content}</h3>
                            <p>by {post.username} at {formattedDate}</p>
                            <button onClick={() => selectPost(post.id)}>view post</button>
                        </li>
                    );
                })}
            </ul>
        </div>
    );
};

export default PostList;