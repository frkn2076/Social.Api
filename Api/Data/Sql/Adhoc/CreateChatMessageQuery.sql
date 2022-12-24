﻿INSERT INTO public.chat_message
	      ( authorid
		  , firstname
		  , lastname
		  , createdat 
		  , messageid
		  , status
		  , text
		  , type
		  , height
		  , width
		  , image_name
		  , size
		  , uri
		  , activityid)
	 VALUES 
	      ( @authorId
		  , @firstName
		  , @lastName
		  , @createdAt
		  , @messageId
		  , @status
		  , @text
		  , @type
		  , @height
		  , @width
		  , @imageName
		  , @size
		  , @uri
		  , @activityId)
  RETURNING *
