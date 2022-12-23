SELECT *
  FROM public.chat_message
 WHERE activityid = @activityId
 ORDER BY 1 DESC