INSERT INTO public.chat_message
	      ( authorid
		  , firstname
		  , lastname
		  , createdat 
		  , messageid
		  , status
		  , text
		  , type
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
		  , @activityId)
  RETURNING *
