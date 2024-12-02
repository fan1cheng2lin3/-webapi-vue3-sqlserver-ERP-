<template>
    <el-dialog v-model="dialogVisible" :title="dialogTitle" width="35%" :before-close="handleClose">
      <el-form :model="ruleFroms" label-width="auto" style="max-width: 600px">
        <el-form-item label="名称" prop="fieldName">
          <el-input v-model="ruleFroms.fieldName" />
        </el-form-item>
        <el-form-item label="值" prop="fieldValue">
          <el-input v-model="ruleFroms.fieldValue" />
        </el-form-item>
      </el-form>
  
      <template #footer>
        <div class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="addCategory">提交</el-button>
        </div>
      </template>
    </el-dialog>
  </template>
  
  <script setup>
  import axios from '@/api/api_config';
  import { reactive, toRefs, watch, inject } from 'vue';
  import { ElMessage } from 'element-plus';
  
  // 定义 props
  const props = defineProps({
    dialogTitle: { type: String },
    tableRow: { type: Object },
    id: {
      type: String,
      required: true
    }
  });
  
  const realod = inject("getList");
  
  const state = reactive({
    dialogVisible: false,
    ruleFroms: {
      id: "",
      fieldName: "",
      fieldValue: "", // 新增描述字段
    },
  });
  
  //监听器
  watch(
    () => props.tableRow,
    () => {
      state.ruleFroms = { ...props.tableRow }; // 自动填充 fieldName 和 fieldValue
    },
    { deep: true, immediate: true }
  );
  
  const dialogCategory = () => {
    state.dialogVisible = true;
  };
  
  // 提交修改
  const addCategory = () => {
  const param2 = {
    categoryId: props.id,       // 父组件传递的分类ID
    fieldName: state.ruleFroms.fieldName,  // 更新的字段名
    fieldValue: state.ruleFroms.fieldValue // 更新的字段值
  };

  if (!props.id) {
    ElMessage.error("缺少有效的分类 ID");
    return; // 提前返回，不继续执行
  }

  if (props.dialogTitle === "添加") {
    // 添加操作
    axios.post(`/CategoryData/${props.id}`, param2)
      .then(() => {
        ElMessage.success("添加成功");
        state.dialogVisible = false; // 关闭窗口
        realod(); // 刷新列表
      })
      .catch((error) => {
        ElMessage.error("添加失败: " + error.response.data.message);
      });
  } else {
    // 修改操作
    axios.put(`/CategoryData/${props.tableRow.id}`, param2)
      .then(() => {
        ElMessage.success("修改成功");
        state.dialogVisible = false; // 关闭窗口
        realod(); // 刷新列表
      })
      .catch((error) => {
        ElMessage.error("修改失败: " + error.response.data.message);
      });
  }
};

  
  //结构
  const { dialogVisible, ruleFroms } = toRefs(state);
  
  // 暴露给父组件的方法
  defineExpose({ dialogCategory });
  </script>
  