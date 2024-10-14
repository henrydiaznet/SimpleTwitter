import React, { useEffect, useState } from 'react';
import { Post } from '../models/post.ts';
import {API_BASE_URL} from "../../config.ts";
import '../App.css';

interface PostDetailsProps {
    postId: number;
    backToList: () => void;
}

const PostDetails: React.FC<PostDetailsProps> = ({ postId, backToList }) => {
    const [post, setPost] = useState<Post | null>(null);

    useEffect(() => {
        fetch(`${API_BASE_URL}/posts/${postId}`)
            .then(response => response.json())
            .then(data => setPost(data))
            .catch(error => console.error('Error fetching post:', error));
    }, [postId]);

    if (!post) {
        return <div>Loading post details...</div>;
    }

    return (
        <div>
            <h1>Post by {post.username}</h1>
            <p>{post.content}</p>
            <p>at {new Date(post.createdDate).toLocaleString()}</p>
            <button onClick={backToList}>Back to All Posts</button>
        </div>
    );
};

export default PostDetails;