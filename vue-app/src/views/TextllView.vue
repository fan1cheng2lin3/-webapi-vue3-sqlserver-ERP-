<template>
  <div>
    <el-table
      :data="tableData"
      style="width: 100%"
      border
      @cell-click="handleCellClick"
    >
      <el-table-column prop="name" label="产品名称" width="120">
        <template #default="scope">
          <div v-if="isEditing(scope.row, 'name')">
            <el-input
              v-model="editData.name"
              size="small"
              @blur="updateField(scope.row, 'name')"
            />
          </div>
          <div v-else>
            {{ scope.row.name }}
          </div>
        </template>
      </el-table-column>
      <el-table-column prop="product_code" label="产品代码" width="120">
        <template #default="scope">
          <div v-if="isEditing(scope.row, 'product_code')">
            <el-input
              v-model="editData.product_code"
              size="small"
              @blur="updateField(scope.row, 'product_code')"
            />
          </div>
          <div v-else>
            {{ scope.row.product_code }}
          </div>
        </template>
      </el-table-column>
      <el-table-column prop="product_type" label="产品类型" width="120">
        <template #default="scope">
          <div v-if="isEditing(scope.row, 'product_type')">
            <el-input
              v-model="editData.product_type"
              size="small"
              @blur="updateField(scope.row, 'product_type')"
            />
          </div>
          <div v-else>
            {{ scope.row.product_type }}
          </div>
        </template>
      </el-table-column>
      <el-table-column prop="supplier_name" label="供应商" width="120">
        <template #default="scope">
          <div v-if="isEditing(scope.row, 'supplier_name')">
            <el-input
              v-model="editData.supplier_name"
              size="small"
              @blur="updateField(scope.row, 'supplier_name')"
            />
          </div>
          <div v-else>
            {{ scope.row.supplier_name }}
          </div>
        </template>
      </el-table-column>
      <el-table-column prop="unit_price" label="单价" width="120">
        <template #default="scope">
          <div v-if="isEditing(scope.row, 'unit_price')">
            <el-input-number
              v-model="editData.unit_price"
              size="small"
              @blur="updateField(scope.row, 'unit_price')"
            />
          </div>
          <div v-else>
            {{ scope.row.unit_price }}
          </div>
        </template>
      </el-table-column>
      <el-table-column prop="count" label="数量" width="120">
        <template #default="scope">
          <div v-if="isEditing(scope.row, 'count')">
            <el-input-number
              v-model="editData.count"
              size="small"
              @blur="updateField(scope.row, 'count')"
            />
          </div>
          <div v-else>
            {{ scope.row.count }}
          </div>
        </template>
      </el-table-column>
    </el-table>
    <div style="margin-top: 20px;">
      <el-button type="primary" @click="saveAllChanges">保存修改</el-button>
      <el-button type="success" @click="addNewRow">新增行</el-button>
    </div>
  </div>
</template>

<script setup>
import { reactive } from "vue";
import { ElMessage } from "element-plus";
import axios from "axios";

// 存储已修改的数据
const updatedData = reactive([]);

// 当前正在编辑的数据
const editData = reactive({
  row: null,
  field: null,
});

// 判断是否正在编辑
const isEditing = (row, field) => editData.row === row && editData.field === field;

// 点击单元格开始编辑
const handleCellClick = (row, column) => {
  // 如果有未保存的编辑数据，先保存
  if (editData.row && editData.field) {
    editData.row[editData.field] = editData[editData.field];
    if (!updatedData.some((item) => item.id === editData.row.id)) {
      updatedData.push({ ...editData.row }); // 添加到修改列表
    }
  }

  // 切换到新的单元格
  editData.row = row;
  editData.field = column.property;
  editData[editData.field] = row[column.property];
};

// 保存单元格编辑数据到本地列表
const updateField = (row, field) => {
  // 保存到当前行数据
  row[field] = editData[field];

  // 确保新增行或修改后的行都被正确加入修改列表
  const existingIndex = updatedData.findIndex((item) => item.id === row.id);
  if (existingIndex === -1) {
    updatedData.push({ ...row }); // 添加新行或修改后的行
  } else {
    updatedData[existingIndex] = { ...row }; // 更新修改列表中的数据
  }

  // 清空编辑状态
  editData.row = null;
  editData.field = null;
};

// 最后一个使用的ID
let lastUsedId = 0;

// 新增行
const addNewRow = () => {
  lastUsedId += 1; // 确保每次调用时ID递增
  const newRow = {
    id: lastUsedId, // 递增的唯一 ID
    name: "",
    product_code: "",
    product_type: "",
    supplier_name: "",
    unit_price: 0,
    count: 0,
  };
  tableData.push(newRow);
  updatedData.push(newRow); // 将新增行直接加入修改列表
};

// 表格数据
const tableData = reactive([
  {
    id: 1, // 假设数据有唯一 ID
    name: "示例产品1",
    product_code: "P001",
    product_type: "电子",
    supplier_name: "供应商A",
    unit_price: 100,
    count: 10,
  },
]);

// 提交前检查所有数据
const saveAllChanges = async () => {
  // 过滤出有效数据（已经存在的和新增的）

  // 打印 tableData 以检查请求数据
  console.log("提交的数据: ", tableData);

  try {
    await axios.post("/File_Management/appproduct", tableData); // 提交所有有效数据
    ElMessage.success("保存成功！");
    updatedData.splice(0); // 清空修改列表
  } catch (error) {
    ElMessage.error("保存失败！");
    console.error("请求失败：", error.response); // 打印错误响应内容
  }
};
</script>

<style scoped>
.el-input,
.el-input-number {
  width: 100%;
}
</style>
